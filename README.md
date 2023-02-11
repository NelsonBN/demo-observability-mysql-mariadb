# Solution to monitor MySQL/MariaDB with Prometheus, Loki and Grafana



## Docker
* [Grafana](https://hub.docker.com/r/grafana/grafana)
* [Prometheus](https://hub.docker.com/r/prom/prometheus)
* [Loki](https://hub.docker.com/r/grafana/loki)
* [Promtail](https://hub.docker.com/r/grafana/promtail)
* [Mysql exporter](https://hub.docker.com/r/prom/mysqld-exporter)
* [Mysql](https://hub.docker.com/_/mysql)
* [MariaDB](https://hub.docker.com/_/mariadb)



## Grafana

### Dashboards
- [MySQL Overview](https://grafana.com/grafana/dashboards/7362-mysql-overview/)
- [Mysql - Prometheus](https://grafana.com/grafana/dashboards/6239-mysql/)



## MySQL

### Create user to export metrics
```sql
CREATE USER 'metrics-exporter'@'%' IDENTIFIED BY 'a123456789';
GRANT PROCESS, REPLICATION CLIENT ON *.* TO 'metrics-exporter'@'%';
GRANT SELECT ON `%`.* TO 'metrics-exporter'@'%';
```

#### Additional commands
```sql
DROP USER 'metrics-exporter'@'%'; -- Delete user
SELECT * FROM information_schema.user_privileges; -- Show privileges of all users
```

### Show variables
```sql
SELECT
    @@general_log, -- 0 or 1 (off or on) / Enabled saving of general query log to the general log file.
    @@general_log_file, -- Path to the general log file
    @@slow_query_log, -- 0 or 1 (off or on) / Enabled saving of slow queries to the slow query log file.
    @@slow_query_log_file, -- Path to the slow query log file
    @@slow_launch_time, -- Time in seconds that to slow connection log file
    @@long_query_time, -- Time in seconds that to slow query log file
    @@log_queries_not_using_indexes, -- 0 or 1 (off or on) / Enabled saving of queries that do not use indexes to the general log file.
    @@log_error, -- Path to the error log file
    @@expire_logs_days  -- Number of days to keep log files before deleting them
;
```

#### Additional commands
```sql
SHOW SESSION VARIABLES; -- Show all session variables
SELECT @@<nome_da_variável>; -- Show value of a specific variable
```



## TODO:
To investigate:
- https://grafana.com/grafana/dashboards/12630-mysql-query-performance-troubleshooting/
