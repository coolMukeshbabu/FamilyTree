# FamilyTree

Problem : 

Our story is set in the planet of Lengaburu......in the distant, distant 
galaxy of Tara B. And our protagonists are King Shan, Queen Anga & 
their family. 
King Shan is the emperor of Lengaburu and has been ruling the 
planet for the last 350 years (they have long lives in Lengaburu, you 
see!). Let’s write some code to get to know the family. 
This coding problem is for backend and fullstack developers.

Write code to model out the King Shan family tree so that: 
• Given a ‘name’ and a ‘relationship’, you should output the people corresponding to the relationship in the order in 
which they were added to the family tree. Assume the names of the family members are unique. 
• You should be able to add a child to any family in the tree through the mother. 
Simple, right? But remember.. our evaluation is based not only on getting the right output, but on how you've written your code. 
Relationships To Handle

There are many relations that could exist but at a minimum, your code needs to handle these relationships.
Relationships => Paternal-Uncle 
Maternal-Uncle 
Paternal-Aunt 
Maternal-Aunt 
Sister-In-Law 
Brother-In-Law 
Son 
Daughter 
Siblings

Sample Input/Output:
Do initialise the existing family tree on program start. Your program should take the location to the 
test file as parameter. Input needs to be read from a text file, and output should be printed to the 
console. The test file will contain only commands to modify or verify the family tree.

Input format to add a child:
ADD_CHILD ”Mother’s-Name" "Child's-Name" 
"Gender"

Input format to find the people belonging to a relationship:
GET_RELATIONSHIP ”Name” “Relationship”

Output format on finding the relationship:
”Name 1” “Name 2”… “Name N” 

Example test file:
ADD_CHILD Chitra Aria Female 
GET_RELATIONSHIP Lavnya Maternal-Aunt 
GET_RELATIONSHIP Aria Siblings

Output on finding the relationship:
CHILD_ADDITION_SUCCEEDED 
Aria 
Jnki Ahit

