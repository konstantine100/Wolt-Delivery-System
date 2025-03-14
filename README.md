<!-- Metadata for LinkedIn and Social Sharing -->
**Project Name:** Wolt Delivery System  
**Project Link:** [Wolt Delivery System](https://github.com/konstantine100/Wolt-Delivery-System)  
**Description:** A comprehensive food and product delivery platform built with Entity Framework, offering dynamic pricing based on distance and weather conditions.  

---

# Wolt Delivery System

A comprehensive food and product delivery platform built with Entity Framework. This system connects customers with restaurants and stores, providing a seamless delivery experience with smart routing and dynamic pricing based on distance and weather conditions.

## Features

### For Venue Owners
- Create and manage venue owner accounts
- Add, edit, and delete restaurants or stores
- Create and manage menus with customizable items
- Set and update product prices
- Access dashboard to track orders and manage inventory

### For Customers
- Create personal accounts with virtual balance
- Search venues by name, food/product type, or category
- Add items to shopping cart
- Place orders with real-time delivery pricing
- Receive order confirmations via email

### Technical Features
- Smart routing between customer and venue locations
- Dynamic pricing based on:
  - Travel distance (calculated via OpenRouteService)
  - Current weather conditions (using OpenWeatherMap)
- Automated order receipts generated as DOCX files
- Email notifications with order details

## Technologies Used

### Core Framework
- Entity Framework for data management

### APIs
- OpenStreetMap - Address-to-coordinate conversion
- OpenRouteService - Route optimization and distance calculation
- OpenWeatherMap - Real-time weather condition data

### Security & Validation
- BCrypt for password hashing and security
- FluentValidation for input validation

### Data Processing
- Newtonsoft.Json for JSON parsing
- RestSharp for API interactions

### Documentation & Notifications
- DocX for generating order receipts
- SMTP integration for email notifications

## How It Works

1. Customer searches for venues or specific food/products
2. Items are added to shopping cart
3. Upon checkout, the system:
   - Converts addresses to coordinates via OpenStreetMap
   - Calculates optimal delivery route via OpenRouteService
   - Determines base delivery fee based on distance
   - Checks current weather conditions via OpenWeatherMap
   - Adjusts pricing if weather impacts delivery difficulty
   - Processes the payment using the customer's virtual balance
   - Generates a detailed receipt using DocX
   - Sends confirmation email with order details to the customer
