# oloapiarc

The API is defined here https://jsonplaceholder.typicode.com/guide

GET https://jsonplaceholder.typicode.com/posts
POST https://jsonplaceholder.typicode.com/posts
PUT https://jsonplaceholder.typicode.com/posts/{postId}
DELETE https://jsonplaceholder.typicode.com/posts/{postId}
POST https://jsonplaceholder.typicode.com/posts/{postId}/comments
GET https://jsonplaceholder.typicode.com/comments?postId={postId}

Acceptance Criteria: (in order of precedence)

The code base is well organized, DRY, easily readable and appropriately commented
The project’s git commit history is well commented and easy to understand.
The tests are reliable, and can be executed numerous times in a row without failures
Happy path tests were built for all major endpoints
Negative path tests were built testing for standard failures
Tests have detailed logs or reports helping to troubleshoot failures
Tests can be executed in parallel
Tests are data driven for maximum coverage
