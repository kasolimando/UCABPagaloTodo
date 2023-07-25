# Stress Tests

The goal of stress testing is to analyze the behavior of the system after a failure. For stress testing to be successful, a system should display an appropriate error message while it is under extreme conditions.

[![Platform](https://img.shields.io/badge/platform-%20win--64-lightgrey)](https://www.educba.com/linux-vs-windows/)
[![JMetter](https://img.shields.io/badge/platform-Jmeter-yellow)](https://jmeter.apache.org/)

## Pre Requisites
* **Git** installed [![Git](https://img.shields.io/badge/Download-grey)](https://git-scm.com/downloads)
* **Java Platform** installed [![Java](https://img.shields.io/badge/Download-grey)](https://www.oracle.com/java/technologies/javase-downloads.html)
* **JMeter Binaries** installed  [![JMeter](https://img.shields.io/badge/Download-grey)](http://jmeter.apache.org/download_jmeter.cgi)

## Set up & Run Tests

* Install JMeter binaries: You simply unzip the zip/tar file into the directory where you want JMeter to be installed. 

**Run tests by Postman Desktop App**

1. Run the file <Localdirectory>/jmeter.bat to start JMeter in GUI mode

2. Open the Testplan

3. Run Test Plan

**Run tests by Command prompt (cmd)**

1. Clone the stress tests repository

```bash
 git clone <repo link>
```

2. Run Jmeter Tests

```bash
 jmeter -n -t <TestPlanName>.jmx - l log.jtl
```