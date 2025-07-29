# Communicating Funding Alpha
This repository contains the source code for the communicating funding alpha, including the prototype, infrastructure definitions and related source code.

## Using the prototype

You can [learn how to use the Prototype Kit on GOV.UK](https://prototype-kit.service.gov.uk/docs/).

To run the prototype:
- Open this repository with GitHub Desktop or a similar tool
- Open the prototype code in an editor such as [Visual Studio Code](https://code.visualstudio.com/)
- To get the kit up and running on your machine, in your terminal you should run:
```
cd prototype
npm install
npm run dev
```

## Design histories
We are [documenting our design decisions and experiences in a design history](https://design-histories.education.gov.uk/communicating-funding-alpha).

## Infrastructure

This repo uses Terraform to define its infrastructure on Azure. See [terraform/README.md](./terraform/README.md) for more information.