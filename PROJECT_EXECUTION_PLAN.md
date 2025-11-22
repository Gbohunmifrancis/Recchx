# Recchx Project - Phased Execution Plan
**Created:** November 5, 2025  
**Architecture:** Clean Architecture with Modular Monolith  
**Stack:** .NET 8, PostgreSQL, Entity Framework Core, Hangfire, OpenAI, Qdrant

---

## 🎯 Project Overview
Recchx is an AI-powered recruitment outreach platform that:
- Matches user profiles with companies using semantic search
- Automates personalized email campaigns via Gmail/Outlook
- Tracks engagement (opens, clicks, replies)
- Manages prospects and contacts

---

## 📋 Phase Breakdown Strategy

### **PHASE 0: Foundation & Infrastructure Setup** ✅
**Goal:** Set up core infrastructure, database, and shared kernel

**Duration:** 3-5 days

#### Tasks:
1. **Database Setup**
   - Install PostgreSQL
   - Create database schema with migrations
   - Set up Entity Framework Core with DbContext
   - Configure connection strings

2. **SharedKernel Module**
   - Base Entity classes (with Id, CreatedAt, UpdatedAt)
   - Result/Response pattern for error handling
   - Common interfaces (IRepository, IUnitOfWork)
   - Domain events infrastructure
   - Value Objects (Email, etc.)

3. **Infrastructure Configuration**
   - Dependency Injection setup
   - Logging (Serilog)
   - Configuration management
   - Exception handling middleware
   - CORS policy

4. **Testing Infrastructure**
   - xUnit setup
   - Integration test base classes
   - Test database configuration

**Deliverables:**
- ✅ Database created with all tables
- ✅ EF Core migrations working
- ✅ SharedKernel reusable components
- ✅ Basic API running with health checks

**Validation:**
- Run migrations successfully
- Seed test data
- Health check endpoint returns 200 OK

---

### **PHASE 1: Users Module - Authentication & Profile Management**
**Goal:** Complete user registration, authentication, and profile management

**Duration:** 5-7 days

#### Tasks:
1. **Domain Layer (Recchx.Users.Domain)**
   - User aggregate (Id, Email, FirstName, LastName, etc.)
   - UserProfile entity with Skills, Summary, Preferences
   - Domain events (UserCreated, ProfileUpdated)
   - Validation rules

2. **Application Layer (Recchx.Users.Application)**
   - Commands:
     - RegisterUserCommand
     - UpdateUserProfileCommand
   - Queries:
     - GetUserByIdQuery
     - GetUserProfileQuery
   - DTOs and mapping (AutoMapper)
   - Validators (FluentValidation)

3. **Infrastructure Layer (Recchx.Users.Infrastructure)**
   - EF Core configurations
   - UserRepository implementation
   - Password hashing (ASP.NET Core Identity)
   - JWT token generation

4. **API Endpoints (Recchx.WebAPI)**
   - POST /api/auth/signup
   - POST /api/auth/login
   - GET /api/user/profile
   - PUT /api/user/profile
   - POST /api/auth/forgot-password
   - POST /api/auth/reset-password

5. **Security**
   - JWT authentication middleware
   - Authorization policies
   - Password reset token generation

**Deliverables:**
- ✅ User registration working
- ✅ Login returns JWT token
- ✅ Profile CRUD operations functional
- ✅ Password reset flow complete
- ✅ Unit tests for domain logic
- ✅ Integration tests for endpoints

**Validation:**
- Register new user via Swagger
- Login and receive JWT
- Update profile with authenticated request
- Test password reset flow

---

### **PHASE 2: Mailbox Module - OAuth Integration**
**Goal:** Connect user mailboxes (Gmail/Outlook) via OAuth 2.0

**Duration:** 5-7 days

#### Tasks:
1. **Domain Layer (Recchx.Mailbox.Domain)**
   - MailboxConnection aggregate
   - Provider enum (Gmail, Outlook)
   - Token encryption/decryption logic
   - Domain events (MailboxConnected, TokenRefreshed)

2. **Application Layer (Recchx.Mailbox.Application)**
   - Commands:
     - InitiateOAuthCommand
     - StoreOAuthTokensCommand
     - RefreshAccessTokenCommand
   - Queries:
     - GetMailboxConnectionQuery
   - OAuth service interfaces

3. **Infrastructure Layer (Recchx.Mailbox.Infrastructure)**
   - Gmail OAuth client
   - Outlook OAuth client
   - Token storage (encrypted)
   - HTTP client for Gmail/Outlook APIs

4. **API Endpoints**
   - POST /api/mailbox/connect
   - GET /api/mailbox/callback
   - GET /api/mailbox/status
   - DELETE /api/mailbox/disconnect

5. **Configuration**
   - Google Cloud Console setup (OAuth credentials)
   - Azure AD setup for Outlook
   - Callback URL configuration
   - Scope definitions

**Deliverables:**
- ✅ OAuth flow working for Gmail
- ✅ OAuth flow working for Outlook
- ✅ Tokens stored securely (encrypted)
- ✅ Token refresh mechanism
- ✅ Integration tests with mock OAuth

**Validation:**
- Complete OAuth flow in browser
- Verify tokens stored in database (encrypted)
- Test token refresh before expiry
- Disconnect mailbox successfully

---

### **PHASE 3: Prospects Module - Companies & Contacts Management**
**Goal:** Manage companies, contacts, and basic CRUD operations

**Duration:** 4-6 days

#### Tasks:
1. **Domain Layer (Recchx.Prospects.Domain)**
   - Company aggregate
   - Contact entity
   - Verification status logic
   - Domain events

2. **Application Layer (Recchx.Prospects.Application)**
   - Commands:
     - CreateCompanyCommand
     - UpdateCompanyCommand
     - CreateContactCommand
     - VerifyContactCommand
   - Queries:
     - GetCompanyByIdQuery
     - SearchCompaniesQuery
     - GetContactsByCompanyQuery
   - Import service (CSV upload)

3. **Infrastructure Layer (Recchx.Prospects.Infrastructure)**
   - Repository implementations
   - Email verification service (Hunter.io integration)
   - CSV parsing service

4. **API Endpoints**
   - GET /api/companies
   - POST /api/companies
   - GET /api/companies/{id}
   - PUT /api/companies/{id}
   - GET /api/contacts
   - POST /api/contacts
   - POST /api/contacts/import (CSV upload)
   - POST /api/contacts/{id}/verify

**Deliverables:**
- ✅ Company CRUD working
- ✅ Contact CRUD working
- ✅ CSV import functional
- ✅ Email verification integrated
- ✅ Unit and integration tests

**Validation:**
- Create companies manually
- Import contacts from CSV
- Verify contact emails
- Query contacts by company

---

### **PHASE 4: Embeddings & Vector Search Infrastructure**
**Goal:** Implement OpenAI embeddings and Qdrant vector database

**Duration:** 5-7 days

#### Tasks:
1. **Shared Infrastructure**
   - OpenAI API client configuration
   - Qdrant client setup
   - Embedding service interface

2. **User Profile Embeddings**
   - Background job to generate user profile embeddings
   - Store embeddings in UserProfiles table
   - Sync embeddings to Qdrant

3. **Company Embeddings**
   - Background job to generate company embeddings
   - Store embeddings in Companies table
   - Sync embeddings to Qdrant

4. **Qdrant Integration**
   - Collection setup for users
   - Collection setup for companies
   - Indexing configuration
   - Search service implementation

5. **Background Jobs (Hangfire)**
   - Hangfire setup and dashboard
   - GenerateUserEmbeddingJob
   - GenerateCompanyEmbeddingJob
   - Job retry policies

6. **API Endpoints**
   - POST /api/user/profile/embed (trigger manually)
   - POST /api/companies/{id}/embed

**Deliverables:**
- ✅ OpenAI embeddings working
- ✅ Qdrant connected and collections created
- ✅ Hangfire jobs running
- ✅ Embeddings generated for users and companies
- ✅ Vector search functional

**Validation:**
- Update user profile → embedding auto-generated
- Create company → embedding auto-generated
- Query Qdrant directly to verify vectors
- Check Hangfire dashboard for job execution

---

### **PHASE 5: Matching Engine - Semantic Search**
**Goal:** Implement AI-powered matching between users and companies

**Duration:** 5-7 days

#### Tasks:
1. **Domain Layer (Recchx.Applications.Domain)**
   - UserCompanyMatch entity
   - Match score calculation logic
   - Filtering preferences

2. **Application Layer (Recchx.Applications.Application)**
   - Commands:
     - TriggerMatchSearchCommand
   - Queries:
     - GetMatchResultsQuery
     - GetMatchStatusQuery
   - Matching service with business logic

3. **Infrastructure Layer**
   - Qdrant search implementation
   - Match scoring algorithm (cosine similarity + filters)
   - Caching for match results (Redis optional)

4. **Background Jobs**
   - PerformSemanticMatchJob
   - Store results in UserCompanyMatches table
   - Job status tracking

5. **API Endpoints**
   - POST /api/match/search
   - GET /api/match/status/{jobId}
   - GET /api/match/results/{jobId}

**Deliverables:**
- ✅ Semantic matching working
- ✅ Filter by user preferences (industry, size, location)
- ✅ Match scores calculated and ranked
- ✅ Results stored and retrievable
- ✅ Performance optimized

**Validation:**
- Trigger match search for a user
- Poll job status until complete
- Retrieve top 50 matches with scores
- Verify matches align with user preferences

---

### **PHASE 6: Email Templates Module**
**Goal:** Create and manage email templates with Scriban templating

**Duration:** 3-5 days

#### Tasks:
1. **Domain Layer (Recchx.Campaigns.Domain)**
   - EmailTemplate entity
   - Template validation rules
   - Scriban template parsing

2. **Application Layer (Recchx.Campaigns.Application)**
   - Commands:
     - CreateTemplateCommand
     - UpdateTemplateCommand
     - DeleteTemplateCommand
   - Queries:
     - GetUserTemplatesQuery
     - GetTemplateByIdQuery
   - Template service (Scriban rendering)

3. **Infrastructure Layer**
   - Repository implementation
   - Scriban template engine setup

4. **API Endpoints**
   - GET /api/templates
   - POST /api/templates
   - GET /api/templates/{id}
   - PUT /api/templates/{id}
   - DELETE /api/templates/{id}
   - POST /api/templates/{id}/preview (render with sample data)

**Deliverables:**
- ✅ Template CRUD working
- ✅ Scriban templating functional
- ✅ Template preview with test data
- ✅ Variable validation

**Validation:**
- Create template with variables ({{FirstName}}, {{CompanyName}})
- Preview template with sample data
- Update and delete templates
- List user's templates

---

### **PHASE 7: Campaign Module - Core Functionality**
**Goal:** Create campaigns, select contacts, assign templates

**Duration:** 5-7 days

#### Tasks:
1. **Domain Layer (Recchx.Campaigns.Domain)**
   - Campaign aggregate
   - CampaignContact entity
   - Status enums (Draft, Sending, Sent, Replied)
   - Campaign metrics calculation

2. **Application Layer (Recchx.Campaigns.Application)**
   - Commands:
     - CreateCampaignCommand
     - UpdateCampaignCommand
     - AddContactsToCampaignCommand
   - Queries:
     - GetUserCampaignsQuery
     - GetCampaignDetailsQuery
     - GetCampaignContactsQuery

3. **Infrastructure Layer**
   - Repository implementations
   - Campaign metrics calculation

4. **API Endpoints**
   - GET /api/campaigns
   - POST /api/campaigns
   - GET /api/campaigns/{id}
   - PUT /api/campaigns/{id}
   - POST /api/campaigns/{id}/contacts (add contacts)
   - GET /api/campaigns/{id}/contacts

**Deliverables:**
- ✅ Campaign CRUD working
- ✅ Add contacts to campaigns
- ✅ Assign templates to campaigns
- ✅ Schedule campaigns
- ✅ Campaign status management

**Validation:**
- Create campaign
- Add multiple contacts
- Assign template
- Schedule campaign for future date
- View campaign details

---

### **PHASE 8: AI Personalization - OpenAI Integration**
**Goal:** Generate personalized email content using OpenAI

**Duration:** 4-6 days

#### Tasks:
1. **Application Layer**
   - PersonalizeEmailCommand
   - OpenAI prompt engineering
   - Context building (user profile + contact + company + template)

2. **Infrastructure Layer**
   - OpenAI ChatGPT API client
   - Prompt templates
   - Response parsing
   - Rate limiting

3. **API Endpoints**
   - POST /api/email/personalize
     - Input: contactId, templateId, userId
     - Output: personalized subject + body

4. **Personalization Logic**
   - Fetch user profile, company info, contact role
   - Build context prompt
   - Call OpenAI API
   - Merge with template
   - Return personalized content

**Deliverables:**
- ✅ OpenAI personalization working
- ✅ Context-aware email generation
- ✅ Preview before sending
- ✅ Error handling for API failures

**Validation:**
- Personalize email for a contact
- Verify content is relevant and personalized
- Test with different templates
- Handle OpenAI API errors gracefully

---

### **PHASE 9: Email Sending - Gmail/Outlook API Integration**
**Goal:** Send personalized emails via connected mailboxes

**Duration:** 5-7 days

#### Tasks:
1. **Infrastructure Layer (Recchx.Mailbox.Infrastructure)**
   - Gmail send email service
   - Outlook send email service
   - Email factory (tracking pixels, unsubscribe links)
   - Rate limiting per provider

2. **Application Layer**
   - SendCampaignCommand
   - Background job: SendCampaignEmailsJob
   - Send individual email logic
   - Retry mechanism for failures

3. **Background Jobs**
   - SendCampaignEmailsJob (processes all campaign contacts)
   - Per-contact email sending with personalization
   - Update CampaignContact status (Pending → Sent)
   - Record SentAt timestamp

4. **Tracking Setup**
   - Embed tracking pixel (1x1 transparent image)
   - Wrap links with tracking URLs
   - Generate unsubscribe link

5. **API Endpoints**
   - PUT /api/campaigns/{id}/send
   - POST /api/campaigns/{id}/send-test (send test email)

6. **Compliance**
   - Add unsubscribe link to footer
   - Include physical address
   - Rate limiting (50 emails/day per user)

**Deliverables:**
- ✅ Send emails via Gmail API
- ✅ Send emails via Outlook API
- ✅ Background job processing
- ✅ Tracking pixels embedded
- ✅ Rate limiting enforced
- ✅ Compliance requirements met

**Validation:**
- Create and send test campaign
- Receive email in inbox
- Verify tracking pixel present
- Check unsubscribe link works
- Verify rate limiting

---

### **PHASE 10: Email Tracking - Opens, Clicks, Replies**
**Goal:** Track email engagement metrics

**Duration:** 5-7 days

#### Tasks:
1. **Tracking Infrastructure**
   - Tracking pixel endpoint: GET /api/tracking/pixel/{campaignContactId}
   - Link redirect endpoint: GET /api/tracking/click/{campaignContactId}/{linkId}
   - Update OpenedAt, ClickedAt in CampaignContacts

2. **Reply Detection**
   - Background job: SyncEmailRepliesJob
   - Poll Gmail API for replies (filter by campaign thread)
   - Poll Outlook API for replies
   - Match replies to CampaignContacts
   - Update RepliedAt, Status="Replied"

3. **Application Layer**
   - Commands:
     - RecordEmailOpenCommand
     - RecordEmailClickCommand
     - RecordEmailReplyCommand
   - Update campaign metrics (OpenCount, ClickCount, ReplyCount)

4. **Background Jobs**
   - SyncEmailRepliesJob (runs every 15 minutes)
   - Update campaign metrics job

5. **API Endpoints**
   - GET /api/tracking/pixel/{id}
   - GET /api/tracking/click/{id}/{linkId}

**Deliverables:**
- ✅ Track email opens
- ✅ Track link clicks
- ✅ Detect and record replies
- ✅ Update campaign metrics
- ✅ Background job syncing replies

**Validation:**
- Send test email
- Open email → verify OpenedAt updated
- Click link → verify ClickedAt updated
- Reply to email → verify RepliedAt updated after sync job
- Check campaign metrics reflect changes

---

### **PHASE 11: Analytics & Reporting**
**Goal:** Provide dashboards and metrics for campaigns

**Duration:** 4-6 days

#### Tasks:
1. **Application Layer**
   - Queries:
     - GetCampaignAnalyticsQuery
     - GetUserAggregatedAnalyticsQuery
   - DTO for analytics data

2. **Infrastructure Layer**
   - Analytics calculation service
   - Aggregation queries (EF Core)

3. **API Endpoints**
   - GET /api/analytics/campaign/{id}
     - Returns: sent, opened, clicked, replied counts & rates
   - GET /api/analytics/user
     - Returns: total campaigns, total emails sent, avg open rate, etc.
   - GET /api/campaigns/{id}/timeline (activity timeline)

4. **Metrics Calculated**
   - Open rate
   - Click rate
   - Reply rate
   - Time-based analytics (opens over time)

**Deliverables:**
- ✅ Campaign-level analytics
- ✅ User-level aggregated analytics
- ✅ Activity timeline
- ✅ Performance metrics

**Validation:**
- View campaign analytics
- Verify calculations correct
- View user dashboard analytics
- Check timeline shows events

---

### **PHASE 12: Admin Panel & User Management**
**Goal:** Admin features for platform management

**Duration:** 3-5 days

#### Tasks:
1. **Domain Layer**
   - Role-based authorization (User, Admin)

2. **Application Layer**
   - Queries:
     - GetAllUsersQuery (admin only)
     - GetPlatformUsageQuery

3. **API Endpoints**
   - GET /api/admin/users
   - GET /api/admin/usage
   - PUT /api/admin/users/{id}/suspend
   - GET /api/admin/campaigns (all campaigns)

4. **Authorization**
   - Policy-based authorization
   - Admin role checks

**Deliverables:**
- ✅ Admin endpoints secured
- ✅ User management
- ✅ Platform usage stats
- ✅ Suspend/activate users

**Validation:**
- Login as admin
- View all users
- View platform usage
- Suspend a user account

---

### **PHASE 13: Contact Verification Integration**
**Goal:** Integrate email verification services

**Duration:** 3-4 days

#### Tasks:
1. **Infrastructure Layer (Recchx.Prospects.Infrastructure)**
   - Hunter.io API client
   - Verification service interface
   - Bulk verification support

2. **Background Jobs**
   - VerifyContactEmailJob
   - Batch verification for imported contacts

3. **Application Layer**
   - Commands:
     - VerifyContactCommand
     - BulkVerifyContactsCommand

4. **API Endpoints**
   - POST /api/contacts/{id}/verify
   - POST /api/contacts/verify-bulk

**Deliverables:**
- ✅ Hunter.io integration
- ✅ Email verification working
- ✅ Bulk verification
- ✅ Update Verified status

**Validation:**
- Verify single contact
- Bulk verify imported contacts
- Check verification status updated

---

### **PHASE 14: Pricing & Subscription (Optional)**
**Goal:** Implement subscription plans and billing

**Duration:** 5-7 days (if needed)

#### Tasks:
1. **Domain Layer**
   - Subscription entity
   - Plan tiers (Free, Pro, Enterprise)
   - Usage limits

2. **Stripe Integration**
   - Stripe checkout
   - Webhook handling
   - Subscription management

3. **API Endpoints**
   - GET /api/pricing
   - POST /api/subscriptions/checkout
   - GET /api/subscriptions/status
   - POST /api/subscriptions/cancel

**Deliverables:**
- ✅ Pricing page data
- ✅ Stripe checkout flow
- ✅ Subscription status tracking
- ✅ Usage enforcement

---

### **PHASE 15: Testing, Performance & Optimization**
**Goal:** Comprehensive testing and performance tuning

**Duration:** 7-10 days

#### Tasks:
1. **Testing**
   - Unit tests (80%+ coverage)
   - Integration tests for all endpoints
   - End-to-end tests for critical flows
   - Load testing (campaign sending)

2. **Performance Optimization**
   - Database indexing review
   - Query optimization
   - Caching strategy (Redis)
   - Background job optimization

3. **Security Audit**
   - OWASP top 10 review
   - Dependency vulnerability scan
   - Token encryption verification
   - Rate limiting review

4. **Documentation**
   - API documentation (Swagger/OpenAPI)
   - README with setup instructions
   - Architecture documentation
   - Deployment guide

**Deliverables:**
- ✅ Comprehensive test suite
- ✅ Performance benchmarks met
- ✅ Security vulnerabilities addressed
- ✅ Complete documentation

---

### **PHASE 16: Deployment & DevOps**
**Goal:** Deploy to production environment

**Duration:** 5-7 days

#### Tasks:
1. **Infrastructure Setup**
   - PostgreSQL database (managed service)
   - Qdrant cloud or self-hosted
   - Redis for caching
   - Hangfire server configuration

2. **CI/CD Pipeline**
   - GitHub Actions or Azure DevOps
   - Automated tests
   - Docker containerization
   - Automated deployments

3. **Hosting**
   - Azure App Service / AWS / DigitalOcean
   - Environment variables configuration
   - SSL certificates
   - Domain setup

4. **Monitoring**
   - Application Insights / Sentry
   - Hangfire dashboard monitoring
   - Database monitoring
   - Alerting setup

**Deliverables:**
- ✅ Production environment live
- ✅ CI/CD pipeline working
- ✅ Monitoring and logging active
- ✅ Backup strategy implemented

---

## 🔧 Technology Stack Summary

### Backend
- **.NET 8** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Primary database
- **Hangfire** - Background jobs
- **MediatR** - CQRS pattern
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping

### External Services
- **OpenAI API** - Embeddings & GPT-4 for personalization
- **Qdrant** - Vector database
- **Gmail API** - Email sending (OAuth 2.0)
- **Outlook API** - Email sending (OAuth 2.0)
- **Hunter.io** - Email verification
- **Stripe** (optional) - Payment processing

### Infrastructure
- **Redis** - Caching (optional but recommended)
- **Serilog** - Logging
- **JWT** - Authentication
- **Swagger** - API documentation

---

## 📊 Key Principles

1. **Build Incrementally** - Each phase delivers working features
2. **Test Before Moving On** - Validate each phase before starting next
3. **Follow Clean Architecture** - Keep concerns separated
4. **Security First** - Implement security from the start
5. **Performance Matters** - Optimize as you build, not after
6. **Document as You Go** - Keep documentation current

---

## 🚦 Phase Completion Checklist

For each phase, ensure:
- [ ] All features implemented
- [ ] Unit tests written and passing
- [ ] Integration tests passing
- [ ] Manual testing completed
- [ ] No critical bugs
- [ ] Code reviewed
- [ ] Documentation updated
- [ ] Deployed to dev/staging environment

---

## 📈 Estimated Timeline

| Phase | Duration | Cumulative |
|-------|----------|------------|
| Phase 0 | 3-5 days | 5 days |
| Phase 1 | 5-7 days | 12 days |
| Phase 2 | 5-7 days | 19 days |
| Phase 3 | 4-6 days | 25 days |
| Phase 4 | 5-7 days | 32 days |
| Phase 5 | 5-7 days | 39 days |
| Phase 6 | 3-5 days | 44 days |
| Phase 7 | 5-7 days | 51 days |
| Phase 8 | 4-6 days | 57 days |
| Phase 9 | 5-7 days | 64 days |
| Phase 10 | 5-7 days | 71 days |
| Phase 11 | 4-6 days | 77 days |
| Phase 12 | 3-5 days | 82 days |
| Phase 13 | 3-4 days | 86 days |
| Phase 14 | 5-7 days | 93 days (optional) |
| Phase 15 | 7-10 days | 103 days |
| Phase 16 | 5-7 days | 110 days |

**Total: ~3.5-4 months** (110 working days)

---

## 🎯 MVP Scope (Phases 0-10)

For a Minimum Viable Product, focus on:
- Phases 0-10: Core functionality
- Skip Phase 12 (Admin panel) initially
- Skip Phase 14 (Subscriptions) initially
- Simplified Phase 15 (Basic testing)

**MVP Timeline: ~2 months (64 days)**

---

## 📝 Next Steps

1. **Review this plan** - Adjust based on priorities
2. **Set up development environment** - IDE, PostgreSQL, tools
3. **Create project board** - Track progress (GitHub Projects, Jira, etc.)
4. **Start Phase 0** - Foundation setup

---

## 🆘 Risk Mitigation

### High-Risk Items:
1. **OAuth Integration** (Phase 2) - Complex, provider-specific
   - Mitigation: Start with one provider, extensive testing
   
2. **Vector Search** (Phase 4-5) - New technology for team
   - Mitigation: Prototype early, have fallback to simple filtering
   
3. **Email Deliverability** (Phase 9) - Spam filters, reputation
   - Mitigation: Follow best practices, warm-up sending, SPF/DKIM setup
   
4. **Reply Detection** (Phase 10) - Complex email threading
   - Mitigation: Start with simple matching, iterate

### Medium-Risk Items:
1. **Rate Limiting** - Balance performance vs. compliance
2. **Performance at Scale** - 1000s of emails per campaign
3. **OpenAI Costs** - Token usage can add up

---

**Ready to start Phase 0?** Let me know when you want to begin implementation!

