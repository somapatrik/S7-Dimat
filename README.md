
# S7-Dimat is now obsolete
<ul>
  <li>At this moment I am working on Dimat v2</li>
  <li>I am about 60% done</li>
  <li>WPF based</li>
  <li>Source code will probably not be open source</li>
  <li>Planned features for v2 include: PLC system functions, error reading, writing variables into PLC, notes and more</li>
  </ul>

# S7-Dimat (v1)
S7 Diagnostic &amp; maintenance tool

![Image of S7 Dimat](https://www.soma-patrik.cz/wp-content/uploads/2019/04/V%C3%BDst%C5%99i%C5%BEek3.png)

S7 Dimat is a simple software for reading Siemens PLC variables. It works almost the same way as VAT table inside Simatic manager. 
It uses ethernet communication, so reading from only Profibus devices is impossible. I have used Sharp7 for communication.

## Why it exists?
So not PLC people can watch variables from PLC and NOT damage any machines. Mainly industrial maintenance teams. 

## What can it read?
At this moment you can read I,Q,M,DB memory.

## What devices?
Simatic S7-300, S7-400, S7-1200, S7-1500

## How do I read?
The same way you would inside Simatic manager VAT.

DB addressing examples: DB50.DBX0.1 | DB50.DBB5 |  DB50.DBW12 |  DB50.DBD30

I/O addressing examples: IB2 |  QW51 |  ID69 |  Q1.5

Merker addressing examples: MB2 |  MW51 |  MID69 |  M1.5
