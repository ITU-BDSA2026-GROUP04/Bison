#!/usr/bin/env bash
#DB is BISONDBPATH if it exists, otherwise tmp/bison.db
 DB="${BISONDBPATH:-${TMPDIR:-/tmp}/mybison.db}"

sqlite3 "$DB" < data/schema.sql
sqlite3 "$DB" < data/dump.sql
