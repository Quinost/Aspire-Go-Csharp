package publisher

import (
	"services/internal/constants"
	router "services/internal/infrastructure/messagerouter"
)

func GetJobResultExchangeConfig() router.ExchangeConfig {
	return router.ExchangeConfig{
		ExchangeName: constants.JobResultQueue,
		ExchangeType: constants.ExchangeFanout,
		RoutingKey:   "#",
		Durable:      true,
	}
}
