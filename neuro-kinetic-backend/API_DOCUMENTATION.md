# Neuro-Kinetic Backend API Documentation

**Base URL:** `https://localhost:7118/api` (Development)  
**Swagger UI:** `https://localhost:7118/swagger`  
**OpenAPI Spec:** `https://localhost:7118/swagger/v1/swagger.json`

---

## 🔐 Authentication

All protected endpoints require a JWT token in the Authorization header:

```
Authorization: Bearer {your-jwt-token}
```

### Registration
```
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!",
  "firstName": "John",
  "lastName": "Doe",
  "institution": "University Name",
  "researchFocus": "Parkinson's Research"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Public",
    "institution": "University Name",
    "researchFocus": "Parkinson's Research"
  }
}
```

### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response:** Same as registration response

### Validate Token
```
POST /api/auth/validate
Authorization: Bearer {token}
```

**Response:**
```json
{
  "valid": true
}
```

---

## 📚 Publications API

### Get All Publications (Paginated)
```
GET /api/publications?pageNumber=1&pageSize=10&sortBy=createdAt&sortOrder=desc&searchTerm=parkinson
```

**Query Parameters:**
- `pageNumber` (optional, default: 1)
- `pageSize` (optional, default: 10, max: 100)
- `sortBy` (optional: "title", "createdAt", "year")
- `sortOrder` (optional: "asc" or "desc", default: "asc")
- `searchTerm` (optional: searches in title, abstract, authors)

**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "title": "Multi-Modal Domain Adaptation for Parkinson's Disease Detection",
      "abstract": "Abstract text...",
      "authors": "Sarah Johnson, Michael Chen",
      "journal": "Journal of Medical AI",
      "year": "2024",
      "doi": "10.1000/xyz123",
      "link": "https://example.com/publication1",
      "type": "Journal",
      "isFeatured": true,
      "tags": "parkinson,ai,domain-adaptation",
      "createdAt": "2024-11-01T12:00:00Z"
    }
  ],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "hasPrevious": false,
  "hasNext": true
}
```

### Get All Publications (Non-Paginated)
```
GET /api/publications/all
```

**Response:**
```json
[
  {
    "id": 1,
    "title": "...",
    ...
  }
]
```

### Get Featured Publications
```
GET /api/publications/featured?pageNumber=1&pageSize=5&sortBy=createdAt&sortOrder=desc
```

### Get Publication by ID
```
GET /api/publications/{id}
```

**Response:**
```json
{
  "id": 1,
  "title": "...",
  "abstract": "...",
  "authors": "...",
  "journal": "...",
  "year": "...",
  "doi": "...",
  "link": "...",
  "type": "Journal",
  "isFeatured": true,
  "tags": "...",
  "createdAt": "2024-11-01T12:00:00Z"
}
```

### Search Publications
```
GET /api/publications/search?query=parkinson
```

### Create Publication (Auth Required: Researcher/Admin)
```
POST /api/publications
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "New Research Publication",
  "abstract": "Abstract text",
  "authors": "Author 1, Author 2",
  "journal": "Journal Name",
  "year": "2024",
  "doi": "10.1000/xyz789",
  "link": "https://example.com/publication",
  "type": "Journal",
  "isFeatured": false,
  "tags": "tag1,tag2"
}
```

### Update Publication (Auth Required: Researcher/Admin)
```
PUT /api/publications/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Updated Title",
  ...
}
```

### Delete Publication (Auth Required: Admin Only)
```
DELETE /api/publications/{id}
Authorization: Bearer {token}
```

---

## 📊 Performance Metrics API

### Get All Metrics
```
GET /api/metrics
```

**Response:**
```json
[
  {
    "id": 1,
    "metricName": "Cross-Domain Accuracy",
    "dataset": "Multi-Site Clinical Dataset",
    "accuracy": 0.87,
    "precision": 0.85,
    "recall": 0.89,
    "f1Score": 0.87,
    "specificity": 0.91,
    "sensitivity": 0.89,
    "domainAdaptationDrop": 0.08,
    "validationMethod": "5-Fold Cross Validation",
    "notes": "Performance across 5 different clinical sites",
    "modelVersion": "v2.1",
    "foldNumber": null,
    "createdAt": "2024-11-01T12:00:00Z"
  }
]
```

### Get Dashboard Metrics
```
GET /api/metrics/dashboard
```
Returns top 20 most recent metrics.

### Get Metric by ID
```
GET /api/metrics/{id}
```

### Get Metrics by Dataset
```
GET /api/metrics/dataset/{datasetName}
```

### Create Metric (Auth Required: Researcher/Admin)
```
POST /api/metrics
Authorization: Bearer {token}
Content-Type: application/json

{
  "metricName": "New Metric",
  "dataset": "Dataset Name",
  "accuracy": 0.85,
  "precision": 0.83,
  "recall": 0.87,
  "f1Score": 0.85,
  "specificity": 0.90,
  "sensitivity": 0.87,
  "domainAdaptationDrop": 0.10,
  "validationMethod": "5-Fold Cross Validation",
  "notes": "Additional notes",
  "modelVersion": "v2.1",
  "foldNumber": 1
}
```

---

## 🗂️ Datasets API

### Get All Datasets
```
GET /api/datasets
```

**Response:**
```json
[
  {
    "id": 1,
    "name": "Multi-Site Clinical Dataset",
    "source": "Various Clinical Sites",
    "version": "1.0",
    "totalSamples": 1250,
    "voiceSamples": 980,
    "gaitSamples": 920,
    "multiModalSamples": 650,
    "description": "Comprehensive dataset...",
    "license": "Research Use Only",
    "accessLink": "https://example.com/dataset1",
    "isPublic": false,
    "citation": "Johnson, S. et al. Multi-Site Clinical Dataset...",
    "createdAt": "2024-11-01T12:00:00Z"
  }
]
```

### Get Public Datasets
```
GET /api/datasets/public
```

### Get Dataset by ID
```
GET /api/datasets/{id}
```

### Create Dataset (Auth Required: Researcher/Admin)
```
POST /api/datasets
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "New Dataset",
  "source": "Source Name",
  "version": "1.0",
  "totalSamples": 500,
  "voiceSamples": 400,
  "gaitSamples": 300,
  "multiModalSamples": 200,
  "description": "Dataset description",
  "license": "Research Use Only",
  "accessLink": "https://example.com/dataset",
  "isPublic": false,
  "citation": "Citation text"
}
```

### Update Dataset (Auth Required: Researcher/Admin)
```
PUT /api/datasets/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Updated Dataset Name",
  ...
}
```

### Delete Dataset (Auth Required: Admin Only)
```
DELETE /api/datasets/{id}
Authorization: Bearer {token}
```

---

## 🔬 Analysis API

### Process Analysis (Demo)
```
POST /api/analysis/process
Content-Type: application/json

{
  "sessionId": "session-123",
  "hasVoiceData": true,
  "hasGaitData": true,
  "voiceDataJson": "{\"features\": [...]}",
  "gaitDataJson": "{\"features\": [...]}",
  "waveformDataJson": "{\"data\": [...]}",
  "skeletonDataJson": "{\"skeleton\": [...]}"
}
```

**Response:**
```json
{
  "sessionId": "session-123",
  "analysisType": "MultiModal",
  "predictionScore": 0.75,
  "confidenceScore": 0.88,
  "predictedClass": "ParkinsonPositive",
  "voiceFeaturesJson": "{\"features\": [...]}",
  "gaitFeaturesJson": "{\"features\": [...]}",
  "waveformDataJson": "{\"data\": [...]}",
  "skeletonDataJson": "{\"skeleton\": [...]}",
  "createdAt": "2024-11-01T12:00:00Z",
  "isSimulation": true
}
```

### Get Analysis by Session ID
```
GET /api/analysis/session/{sessionId}
```

### Get Recent Analyses
```
GET /api/analysis/recent?count=10
```

**Query Parameters:**
- `count` (optional, default: 10)

---

## ✅ Cross-Validation Results API

### Get All Cross-Validation Results
```
GET /api/crossvalidation
```

**Response:**
```json
[
  {
    "id": 1,
    "datasetName": "Multi-Site Clinical Dataset",
    "validationMethod": "5-Fold Cross Validation",
    "foldNumber": 1,
    "accuracy": 0.86,
    "precision": 0.84,
    "recall": 0.88,
    "f1Score": 0.86,
    "domainAdaptationDrop": 0.09,
    "sourceSite": "Site A",
    "targetSite": "Site B",
    "modelVersion": "v2.1",
    "notes": "Fold 1: Site A to Site B",
    "createdAt": "2024-11-01T12:00:00Z"
  }
]
```

### Get Cross-Validation Result by ID
```
GET /api/crossvalidation/{id}
```

### Get Results by Dataset
```
GET /api/crossvalidation/dataset/{datasetName}
```

### Create Cross-Validation Result (Auth Required: Researcher/Admin)
```
POST /api/crossvalidation
Authorization: Bearer {token}
Content-Type: application/json

{
  "datasetName": "Dataset Name",
  "validationMethod": "5-Fold Cross Validation",
  "foldNumber": 1,
  "accuracy": 0.86,
  "precision": 0.84,
  "recall": 0.88,
  "f1Score": 0.86,
  "domainAdaptationDrop": 0.09,
  "sourceSite": "Site A",
  "targetSite": "Site B",
  "modelVersion": "v2.1",
  "notes": "Additional notes"
}
```

---

## 🤝 Collaboration API

### Create Collaboration Request (Public)
```
POST /api/collaboration
Content-Type: application/json

{
  "institutionName": "Research Institution",
  "contactName": "John Doe",
  "contactEmail": "john@institution.com",
  "contactPhone": "+1234567890",
  "proposalDescription": "We would like to collaborate...",
  "collaborationType": "Data Sharing"
}
```

**Response:**
```json
{
  "id": 1,
  "institutionName": "Research Institution",
  "contactName": "John Doe",
  "contactEmail": "john@institution.com",
  "contactPhone": "+1234567890",
  "proposalDescription": "We would like to collaborate...",
  "collaborationType": "Data Sharing",
  "status": "Pending",
  "createdAt": "2024-11-01T12:00:00Z"
}
```

### Get All Collaboration Requests (Auth Required: Admin Only)
```
GET /api/collaboration
Authorization: Bearer {token}
```

### Get Collaboration Request by ID (Auth Required: Admin Only)
```
GET /api/collaboration/{id}
Authorization: Bearer {token}
```

### Update Collaboration Request Status (Auth Required: Admin Only)
```
PUT /api/collaboration/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "Approved",
  "responseNotes": "Approved for collaboration"
}
```

**Status Values:**
- `Pending`
- `Approved`
- `Rejected`
- `UnderReview`

---

## 👥 User Roles

| Role | Description | Access Level |
|------|-------------|--------------|
| `Public` | Default user role | Read-only access to public endpoints |
| `Researcher` | Research team member | Can create/edit publications, metrics, datasets |
| `MedicalProfessional` | Healthcare professional | Read access + special features |
| `Admin` | System administrator | Full access including user management |

---

## 🔒 Authorization Rules

### Public Endpoints (No Auth Required)
- `GET /api/publications/*`
- `GET /api/metrics/*`
- `GET /api/datasets/*`
- `GET /api/crossvalidation/*`
- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/analysis/process`
- `POST /api/collaboration` (create request)

### Researcher/Admin Only
- `POST /api/publications`
- `PUT /api/publications/{id}`
- `POST /api/metrics`
- `POST /api/datasets`
- `PUT /api/datasets/{id}`
- `POST /api/crossvalidation`

### Admin Only
- `DELETE /api/publications/{id}`
- `DELETE /api/datasets/{id}`
- `GET /api/collaboration` (view all requests)
- `PUT /api/collaboration/{id}/status` (update status)

---

## 📝 Common Response Formats

### Success Response
```json
{
  "items": [...],
  "totalCount": 100,
  ...
}
```

### Error Response
```json
{
  "message": "Error description"
}
```

### Validation Error Response
```json
{
  "errors": {
    "email": ["Email is required"],
    "password": ["Password must be at least 6 characters"]
  }
}
```

---

## 🚨 HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized (missing/invalid token) |
| 403 | Forbidden (insufficient permissions) |
| 404 | Not Found |
| 500 | Internal Server Error |

---

## 📦 Frontend Integration Examples

### Angular Service Example
```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7118/api';
  private token: string | null = null;

  constructor(private http: HttpClient) {}

  // Set token after login
  setToken(token: string) {
    this.token = token;
    localStorage.setItem('token', token);
  }

  // Get auth headers
  private getHeaders(): HttpHeaders {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    
    if (this.token) {
      return headers.set('Authorization', `Bearer ${this.token}`);
    }
    
    return headers;
  }

  // Login
  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/auth/login`, {
      email,
      password
    });
  }

  // Get Publications (Paginated)
  getPublications(page: number = 1, pageSize: number = 10, searchTerm?: string): Observable<any> {
    let url = `${this.baseUrl}/publications?pageNumber=${page}&pageSize=${pageSize}`;
    if (searchTerm) {
      url += `&searchTerm=${encodeURIComponent(searchTerm)}`;
    }
    return this.http.get(url);
  }

  // Get Publication by ID
  getPublication(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/publications/${id}`);
  }

  // Create Publication (Auth Required)
  createPublication(data: any): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/publications`,
      data,
      { headers: this.getHeaders() }
    );
  }

  // Process Analysis
  processAnalysis(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/analysis/process`, data);
  }

  // Get Metrics
  getMetrics(): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics`);
  }

  // Get Datasets
  getDatasets(): Observable<any> {
    return this.http.get(`${this.baseUrl}/datasets`);
  }
}
```

### React Hook Example
```typescript
import { useState, useEffect } from 'react';

export const useApi = () => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem('token')
  );

  const baseUrl = 'https://localhost:7118/api';

  const fetchWithAuth = async (endpoint: string, options: RequestInit = {}) => {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
      ...options.headers,
    };

    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const response = await fetch(`${baseUrl}${endpoint}`, {
      ...options,
      headers,
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.json();
  };

  const login = async (email: string, password: string) => {
    const data = await fetchWithAuth('/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    });
    
    if (data.token) {
      setToken(data.token);
      localStorage.setItem('token', data.token);
    }
    
    return data;
  };

  const getPublications = async (page: number = 1, pageSize: number = 10) => {
    return fetchWithAuth(`/publications?pageNumber=${page}&pageSize=${pageSize}`);
  };

  return {
    login,
    getPublications,
    token,
  };
};
```

### Fetch API Example
```javascript
// Login
const login = async (email, password) => {
  const response = await fetch('https://localhost:7118/api/auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password }),
  });
  
  const data = await response.json();
  
  if (data.token) {
    localStorage.setItem('token', data.token);
  }
  
  return data;
};

// Get Publications with Pagination
const getPublications = async (page = 1, pageSize = 10) => {
  const token = localStorage.getItem('token');
  
  const response = await fetch(
    `https://localhost:7118/api/publications?pageNumber=${page}&pageSize=${pageSize}`,
    {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    }
  );
  
  return response.json();
};
```

---

## 🌐 CORS Configuration

The API is configured to accept requests from:
- `http://localhost:4200` (Angular default)
- `https://localhost:4200`

For production, update CORS settings in `Program.cs`.

---

## 📋 Test Credentials

**Admin:**
- Email: `admin@neurokinetic.com`
- Password: `Admin123!`

**Researcher:**
- Email: `researcher@neurokinetic.com`
- Password: `Researcher123!`

**Medical Professional:**
- Email: `doctor@neurokinetic.com`
- Password: `Doctor123!`

---

## 🔗 Quick Links

- **Swagger UI:** `https://localhost:7118/swagger`
- **OpenAPI JSON:** `https://localhost:7118/swagger/v1/swagger.json`
- **Health Check:** (To be implemented)

---

## 📞 Support

For API issues or questions:
1. Check Swagger UI for interactive testing
2. Review error messages in responses
3. Verify token is included for protected endpoints
4. Check user role permissions

---

**Last Updated:** November 1, 2024  
**API Version:** v1  
**Documentation Version:** 1.0

