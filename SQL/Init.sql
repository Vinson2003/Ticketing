USE TicketingSystemDB;
GO

CREATE TABLE mt_role
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL,
    name NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE mt_user
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL,
    password NVARCHAR(255) NOT NULL,
    name NVARCHAR(150) NOT NULL,
    email NVARCHAR(150) NULL,
    role_id INT NOT NULL,
    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_mt_user_mt_role
        FOREIGN KEY (role_id)
        REFERENCES mt_role(id)
);
GO

CREATE TABLE mt_ticket_category
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL,
    name NVARCHAR(100) NOT NULL,
    is_active BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE mt_ticket_priority
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL,
    name NVARCHAR(100) NOT NULL,
    sort_order INT NOT NULL
);
GO

CREATE TABLE mt_ticket_status
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL,
    name NVARCHAR(100) NOT NULL,
    sort_order INT NOT NULL
);
GO

CREATE TABLE tr_ticket
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ticket_no NVARCHAR(50) NOT NULL,

    title NVARCHAR(200) NOT NULL,
    description NVARCHAR(MAX) NULL,

    category_id INT NOT NULL,
    priority_id INT NOT NULL,
    status_id INT NOT NULL,

    created_by INT NOT NULL,
    assigned_to INT NULL,

    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    resolved_at DATETIME2 NULL,
    closed_at DATETIME2 NULL,
    is_deleted BIT NOT NULL
        CONSTRAINT DF_tr_ticket_is_deleted DEFAULT 0

    CONSTRAINT FK_tr_ticket_category FOREIGN KEY (category_id)
        REFERENCES mt_ticket_category(id),

    CONSTRAINT FK_tr_ticket_priority FOREIGN KEY (priority_id)
        REFERENCES mt_ticket_priority(id),

    CONSTRAINT FK_tr_ticket_status FOREIGN KEY (status_id)
        REFERENCES mt_ticket_status(id),

    CONSTRAINT FK_tr_ticket_created_by FOREIGN KEY (created_by)
        REFERENCES mt_user(id),

    CONSTRAINT FK_tr_ticket_assigned_to FOREIGN KEY (assigned_to)
        REFERENCES mt_user(id)
);
GO

CREATE TABLE tr_ticket_history
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ticket_id INT NOT NULL,
    action VARCHAR(50) NOT NULL,
    description VARCHAR(500) NOT NULL,
    created_by INT NOT NULL,
    created_at DATETIME2 NOT NULL,

    CONSTRAINT FK_tr_ticket_history_ticket
        FOREIGN KEY (ticket_id)
        REFERENCES tr_ticket(id),

    CONSTRAINT FK_tr_ticket_history_user
        FOREIGN KEY (created_by)
        REFERENCES mt_user(id)
);
GO

CREATE TABLE mt_permission
(
    id INT IDENTITY(1,1) PRIMARY KEY,

    description VARCHAR(100) NOT NULL,
    display VARCHAR(150) NOT NULL,

    seq INT NOT NULL DEFAULT 0,
    sub_seq INT NOT NULL DEFAULT 0
);
GO

CREATE TABLE mt_rolepermission
(
    id INT IDENTITY(1,1) PRIMARY KEY,

    role_id INT NOT NULL,
    permission_id INT NOT NULL,

    CONSTRAINT FK_mt_rolepermission_mt_role
        FOREIGN KEY (role_id)
        REFERENCES mt_role(id),

    CONSTRAINT FK_mt_rolepermission_mt_permission
        FOREIGN KEY (permission_id)
        REFERENCES mt_permission(id),

    CONSTRAINT UQ_mt_rolepermission
        UNIQUE(role_id, permission_id)
);
GO