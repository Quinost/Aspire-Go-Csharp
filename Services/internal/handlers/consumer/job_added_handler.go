package consumer

import (
	"context"
	"encoding/json"
	"log"
	"services/internal/constants"
	router "services/internal/infrastructure/messagerouter"
	"services/pkg/generic"
	"services/pkg/models"
	"time"

	"github.com/ThreeDotsLabs/watermill/message"
	"github.com/google/uuid"
)

type JobAddedHandler struct {
	publisher message.Publisher
	pool      *generic.WorkerPool
}

func NewJobAddedHandler(publisher message.Publisher, pool *generic.WorkerPool) *JobAddedHandler {
	return &JobAddedHandler{
		publisher: publisher,
		pool:      pool,
	}
}

func (h *JobAddedHandler) GetQueueConfig() router.QueueConfig {
	return router.QueueConfig{
		QueueName:    constants.JobAddedQueue,
		ExchangeName: constants.JobAddedQueue,
		ExchangeType: constants.ExchangeFanout,
		RoutingKey:   "#",
		Durable:      true,
	}
}

func (h *JobAddedHandler) Handle(_ context.Context, msg *message.Message) error {
	var event models.JobAddedEvent
	if err := json.Unmarshal(msg.Payload, &event); err != nil {
		return err
	}

	h.pool.SubmitJob(func() error {
		log.Printf("Processing job: ID=%s, Title=%s", event.JobID, event.Name)

		time.Sleep(17 * time.Second)

		result := models.JobResultEvent{
			JobID:         event.JobID,
			Name:          event.Name,
			Status:        models.Failed,
			Reason:        string(models.Completed),
			CreatedAtUTC:  event.CreatedAtUTC,
			FinishedAtUTC: time.Now(),
		}
		if err := h.publishJobResult(result); err != nil {
			return err
		}
		return nil
	})

	log.Printf("Job: ID=%s, Title=%s added to workerpool", event.JobID, event.Name)

	return nil
}

func (h *JobAddedHandler) publishJobResult(result models.JobResultEvent) error {
	payload, err := json.Marshal(result)
	if err != nil {
		return err
	}

	msg := message.NewMessage(uuid.New().String(), payload)
	msg.Metadata.Set("content-type", "application/json")

	return h.publisher.Publish(constants.JobResultQueue, msg)
}
