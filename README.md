# Microservices-Assignment
An E-Commerce Application

I have broken down the application into the following microservices:
1.	Product Inventory Service: This microservice will handle the addition and removal of products from the inventory. It will manage the product database and allow the admin to perform CRUD operations on it.
2.	Product Details Service: This microservice will manage the product details like size, price, and design. It will allow the admin to add or remove product details and maintain the database of product details.
3.	Price Service: This microservice will manage the pricing information for each product. It will maintain a database of all product prices and provide the necessary information to other services as required.
4.	Product Service: This microservice will handle the product listing functionality. It will provide a list of all the products available in the inventory and allow the user list products.

In this architecture, the Product Inventory Service and Product Details Service will provide any product-related information. The Price Service will provide the pricing information to Product Details Service. The Product Service will fetch the product data from the Product Inventory Service and display it to the user.

