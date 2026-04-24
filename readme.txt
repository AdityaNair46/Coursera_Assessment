I used GitHub Copilot to generate the LINQ queries for the PUT and DELETE methods. 
When I encountered a NullReferenceException during testing, I used the /fix command in 
Copilot Chat, which suggested adding the null-check is User user ? in my GET route.