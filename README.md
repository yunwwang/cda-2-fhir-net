# CDA to FHIR Converter

## Introduction ##
CDA to FHIR converter takes a CDA/CCD document as input. Parse the content to a FHIR bundle.

## Limitation ##
This library currently support the follow sections from CDA document:
* Header

To add more sections, please submit a request.

## Getting Started ##
The Xml serializer takes an XDocument as input parameter and returns a ClinicalDocument instance. 

### To parse whole document ###
```csharp
var xml = XDocument.Load("test.xml");
var bundle = new CdaParser().Convert(xml);
```


      