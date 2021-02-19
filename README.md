# NetDemo Server

This is a sample application server intended for use with:

- .NET 4.7.2+
- Visual Studio Enterprise 2017

The following info is all you need to create a CI/CD pipeline for this application component within Guide-Rails&#174;.

## Pipeline Definition

We've already created a pipeline definition for this library in `/ci/guide-rails.json`. Once onboarded to Guide-Rails&#174;, your pipeline will consist of the following segments:

- Build - pulls build dependencies (netdemo-sharedlib), compiles, unit tests and packages the application
- Integration - provisions ephemeral testing environment (network, vms, etc), configures deployment target, deploys and installs server application, deploys test runner (Jmeter), runs integration and performance tests
- Production - provisions new deployment target (canary), configures target, deploys and installs application, runs healthchecks, destroys server(s) running previous version of server application

## Onboarding This Repository

1. Fork this repository and add to your preferred source code management server (Bitbucket, Github, Gitlab, etc.) 
2. Log into Guide-Rails&#174; and head to the Configuration Console (aka Onboarding) and add a new Component
3. In the Component configuration view, choose your source code management server and enter your repository clone URL (https or SSH)
4. Verify the properties described in the rest of this document are set properly in your configuration

### Code Analysis

| Name | Value | Description |
| ---- | ----- | ----------- |
| Run Code Analysis | [&#x2713;] | |
| Source Directories | local-sonar-project.properties | Temporarily set this value, should not need to in the future. Proper usage is not to set this value. |
| Additional Properties Files | local-sonar-project.properties | sonar project properties file |

>**Note: Existing Quality Gates**
>
> If this pipeline fails to pass the Build segment because of Quality Gates (ie. unit test code coverage does not meet thresholds, you
can override them in the Component settings.

### Properties

Add the below properties in the Properties section. If desired, these properties can be set on a parent resource to avoid having to set the same values in different repositories.

| Name | Type | Value |
| ---- | ---- | ----- |
| consul.tag | string | ((application.shortsha)) | |
| consul.servicename | string | ((application.name)) | |
| demoserver.port | Number | 8080 | Port to be used by the server. This setting is referenced in the start script |
| performance.limit | Number | 60 | The upper limit of the number of milliseconds the performance test will use as a benchmark for passing the segment. The script itself defaults to 10ms. This can be defined on any level but should be defined on the component level. |

### Deployment Files

Each file listed in deployment files will be passed along to downstream segments that come after the Build segment.

| Name | Description |
| ---- | ----------- |
| ci/start.ps1 | Start script used to launch the server |
| ci/stop.ps1 | Script that stops the service from running |
| ci/service_registry.erb | Configuration file to add the service to the service registry so it can be accessed by a DNS lookup. |
| ci/validatept.ps1 | This script validates if the performance test falls within the performance limit. |
| ci/integration-test.ps1 | Validates that the server responds to requests. |
| netdemo-Server/pt.jmx | Jmeter performance test script |

### Integration Segment

#### Job Steps

The Job Steps perform a few tasks based upon the *demoserver.port* property:

- Add a Firewall rule to allow an exception for the server port.
- Run the integration test script.
- Run the Jmeter performance test.

#### Instance Groups

Set the *Number of Instances* as desired.

Set the dependencies for ((application.name)), in order, as in the table below. Note that some of the properties may already be set with their default values.

| Release | Version | Job Name | Properties |
| - | - | - | - |
| bosh-dns-windows | latest | bosh-dns-windows | |
| | | | address : ((consul.joinservers)) |
| consul-windows | latest | consul-windows | |
| | | | consul.servicename : ((consul.servicename)) |
| | | | consul.domain : ((consul.domain)) |
| | | | consul.environment : ((consul.environment)) |
| | | | consul.joinservers : ((consul.joinservers)) |
| | | | consul.datacenter : ((consul.datacenter)) |
| | | | calculi.intermediate.cert : grsecret:tls.cert |
| | | | calculi.intermediate.key : grsecret:tls.key |
| | | | consul.server : false |
| jmeter | latest | jmeter | |

### Production Segment

The server will be accessible via `http://netdemo-server.service.consul:8080`. Note that the port will change based upon the *demoserver.port* property.

#### Job Steps

The Job Steps perform a couple of tasks based upon the *demoserver.port* property:

- Add a Firewall rule to allow an exception for the server port.
- Run the Jmeter performance test.

#### Instance Groups

Set the *Number of Instances* as desired.

All other settings will be the same as the Integration Instance Group.

#### Persistence

Set to false if you do not want to keep the server running after the segment completes. Set to true if you want to utilize the server after the segment completes. For Production segments, the default setting is true. Check this setting if you expect the server to be running in Production but it is not.
