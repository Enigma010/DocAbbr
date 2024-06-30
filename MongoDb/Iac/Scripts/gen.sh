#!/bin/sh
chmod u+rwx /data
rm -Rf /data/replica.key
openssl rand -base64 741 > /data/replica.key