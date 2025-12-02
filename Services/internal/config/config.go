package config

import (
	"os"

	"gopkg.in/yaml.v2"
)

type Config struct {
	RabbitMq ConnectionCfg `yaml:"rabbitmq"`
}

type ConnectionCfg struct {
	ConnectionString string `yaml:"connectionString"`
}

func Load(path string) (*Config, error) {
	f, err := os.Open(path)

	if err != nil {
		return nil, err
	}

	defer f.Close()

	var cfg Config
	decoder := yaml.NewDecoder(f)
	err = decoder.Decode(&cfg)

	if rabbitMqUrl := os.Getenv("RabbitMq__ConnectionString"); rabbitMqUrl != "" {
		cfg.RabbitMq.ConnectionString = rabbitMqUrl
	}

	return &cfg, err
}