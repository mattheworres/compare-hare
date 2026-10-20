# Local services

PostgreSQL 18, Redis and a Mailpit SMTP sink for local development. All ports bind to `127.0.0.1` only.

## Start

```sh
cp docker/.env.example docker/.env   # first time only
docker compose -f docker/compose.yaml up -d
docker compose -f docker/compose.yaml ps   # postgres, redis, mail should be healthy
```

## Stop

```sh
docker compose -f docker/compose.yaml down        # keeps the Postgres volume
docker compose -f docker/compose.yaml down -v     # reset: also drops the Postgres volume
```

## Ports

| Service  | Address                | Notes                                   |
|----------|------------------------|-----------------------------------------|
| Postgres | `127.0.0.1:5432`       | db `comparehare`, creds from `docker/.env` |
| Redis    | `127.0.0.1:6379`       | no persistence                          |
| SMTP     | `127.0.0.1:1025`       | accepts any mail, no auth               |
| Mail UI  | http://127.0.0.1:8025  | view captured messages                  |

Override host ports with the variables in `docker/.env.example`.

Connection string with the example credentials:
`postgres://comparehare:comparehare_dev@127.0.0.1:5432/comparehare`
