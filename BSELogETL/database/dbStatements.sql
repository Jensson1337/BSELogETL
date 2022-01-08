CREATE TABLE log_entries
(
    id            INTEGER PRIMARY KEY AUTOINCREMENT,
    ip_address    VARCHAR(15)  NOT NULL,
    http_method   VARCHAR(7)   NOT NULL,
    http_location VARCHAR(255) NOT NULL,
    http_code     INTEGER      NOT NULL,
    requested_at  DATETIME     NOT NULL,
    package_size  INTEGER
);

CREATE TABLE filenames
(
    id       INTEGER PRIMARY KEY AUTOINCREMENT,
    filename VARCHAR(255) UNIQUE NOT NULL
);

-- Analyse 1

SELECT *
FROM log_entries
WHERE STRFTIME(requested_at, '%Y') < '2020'
  AND STRFTIME(requested_at, '%Y') > '2020'
  AND (
        ip_address = 'aa'
        OR ip_address = 'bb'
    );


-- Analyse 2

SELECT ip_address, COUNT() AS count
FROM log_entries
WHERE STRFTIME(requested_at, '%Y') < '2020'
  AND STRFTIME(requested_at, '%Y') > '2020'
  AND (
        ip_address = 'aa'
        OR ip_address = 'bb'
    )
GROUP BY ip_address;


-- Analyse 3

SELECT http_method, COUNT() AS count
FROM log_entries
WHERE http_method = ''
  AND STRFTIME(requested_at, '%Y') < '2020'
  AND STRFTIME(requested_at, '%Y') > '2020'
  AND (
        ip_address = 'aa'
        OR ip_address = 'bb'
    )
GROUP BY http_method;


-- Analyse 4

SELECT http_code, COUNT(*) AS count
FROM log_entries
WHERE http_code = ''
  AND STRFTIME(requested_at, '%Y') < '2020'
  AND STRFTIME(requested_at, '%Y') > '2020'
  AND (
        ip_address = 'aa'
        OR ip_address = 'bb'
    )
GROUP BY http_code