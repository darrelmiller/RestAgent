# RESTAgent #

RESTAgent is an alternative to using HTTP Client libraries that makes it easier to consume hypermedia driven web applications. It achieves this goal in a few ways:

- by providing a standardized way to consume links from retrieved representations.  
- requiring developers to handle responses by providing "response actions" based purely on the response and not on the context of the request.
- automaticallly managing the current state of the client server interaction.

