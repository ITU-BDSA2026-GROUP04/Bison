#!/usr/bin/env bash
#DB is BISONDBPATH if it exists, otherwise tmp/bison.db
 DB="${BISONDBPATH:-$([ -f "../bison.db" ] && echo "../bison.db" || echo "${TMPDIR:-/tmp}/bison.db")}"

sqlite3 $DB < ../data/schema.sql
sqlite3 $DB < ../data/dump.sql
