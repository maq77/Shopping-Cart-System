# Shopping Cart API

This project implements a simple shopping cart system in ASP.NET Core with the following features:
- Add items to cart
- Remove items from cart
- View cart contents
- Process a mock checkout

## Getting Started

### Prerequisites
- .NET 6 SDK or later
- Visual Studio 2022 or VS Code

### Running the Project
1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet restore`
4. Run `dotnet build`
5. Run `dotnet run`
6. Access Swagger UI at `https://localhost:7001/swagger` (port may vary)

## API Endpoints

### 1. View Cart
**GET /api/cart**

Returns the current user's cart contents.

**Headers:**
- X-User-Id: (optional) User identifier for cart tracking

**Response:**
```json
{
  "userId": "test-user-123",
  "items": [
    {
      "productId": 1,
      "productName": "Product 1",
      "quantity": 2,
      "priceAtTimeOfAdd": 10.99,
      "subtotal": 21.98
    }
  ],
  "total": 21.98
}
```

### 2. Add to Cart
**POST /api/cart/add**

Adds items to the user's cart.

**Headers:**
- X-User-Id: (optional) User identifier for cart tracking

**Request Body:**
```json
{
  "productId": 1,
  "quantity": 2
}
```

**Response:**
Returns the updated cart (same format as GET /api/cart).

### 3. Update Quantity
**POST /api/cart/UpdateQuantity**

Updates Quantity of item in the user's cart.

**Headers:**
- X-User-Id: (optional) User identifier for cart tracking

**Request Body:**
```json
{
  "productId": 1,
  "quantity": 2
}
```

**Response:**
Returns the updated cart (same format as GET /api/cart).

### 4. Remove from Cart
**POST /api/cart/remove**

Removes items from the user's cart.

**Headers:**
- X-User-Id: (optional) User identifier for cart tracking

**Request Body:**
```json
{
  "productId": 1,
  "quantity": 1  // Optional. If omitted or >= current quantity, removes the item entirely
}
```

**Alternative Endpoint:**
**DELETE /api/cart/{productId}**

Removes the item completely from the cart.

**Response:**
Returns the updated cart (same format as GET /api/cart).

### 5. Checkout
**POST /api/cart/checkout**

Processes a mock checkout.

**Headers:**
- X-User-Id: (optional) User identifier for cart tracking

**Request Body:**
```json
{
  "paymentMethod": "Credit Card",  // Optional
  "shippingAddress": "123 Main St" // Optional
}
```

**Response:**
```json
{
  "orderId": "a1b2c3d4e5f6",
  "total": 21.98,
  "items": [
    {
      "productId": 1,
      "productName": "Product 1",
      "quantity": 2,
      "priceAtTimeOfAdd": 10.99,
      "subtotal": 21.98
    }
  ],
  "paymentSuccessful": true,
  "message": "Payment processed successfully."
}
```

## Error Handling

The API returns appropriate HTTP status codes:
- 200 OK: Successful operation
- 400 Bad Request: Invalid input (e.g., product not found, negative quantity)
- 500 Internal Server Error: Unexpected errors

Error responses include a message field:
```json
{
  "message": "Product with ID 999 not found."
}
```

## Testing

Run the unit tests with:
```
dotnet test
```
it will get erros at beginning due to some issues in MSTest
so run every test indvidual for better performance.

## Available Products (for Testing)

The API includes a mock product catalog with the following items:
- ID: 1, Name: "Product 1", Price: $10.99
- ID: 2, Name: "Product 2", Price: $24.99
- ID: 3, Name: "Product 3", Price: $5.49
- ID: 4, Name: "Product 4", Price: $99.99
- ID: 5, Name: "Product 5", Price: $49.99