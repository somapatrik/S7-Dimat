# S7-Dimat
S7 Diagnostic &amp; maintenance tool

S7 Dimat is a simple software for reading Siemens PLC variables. It works almost the same way as VAT table inside Simatic manager. 
It uses ethernet communication, so reading from only Profibus devices is impossible. I have used Sharp7 for communication.

## Why it exists?
So not PLC people can watch variables from PLC and NOT damage my machines. Mainly industrial maintenance team. 

## What can it read?
At this moment you can read I,Q,M,DB memory.

## What devices?
Simatic S7-300, S7-400, S7-1200, S7-1500

## How do I read?
The same way you would inside Simatic manager VAT.

DB addressing examples: DB50.DBX0.1 | DB50.DBB5 |  DB50.DBW12 |  DB50.DBD30

I/O addressing examples: IB2 |  QW51 |  ID69 |  Q1.5

Merker addressing examples: MB2 |  MW51 |  MID69 |  M1.5
