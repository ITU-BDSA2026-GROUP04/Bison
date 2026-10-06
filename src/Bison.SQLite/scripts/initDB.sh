#!/usr/bin/env bash
#DB is BISONDBPATH if it exists, otherwise tmp/bison.db
 DB="${BISONDBPATH:-${TMPDIR:-/tmp}/mybison.db}"

sqlite3 "../mybison.db" < ../data/schema.sql
sqlite3 "../mybison.db" < ../data/dump.sql
