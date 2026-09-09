USE TicketingSystemDB;
GO

-- Roles
INSERT INTO mt_role (code, name)
VALUES
('ADMIN', 'Administrator'),
('DEVELOPER', 'Developer'),
('USER', 'User');
GO

-- Ticket Categories
INSERT INTO mt_ticket_category (code, name, is_active)
VALUES
('BUG', 'Bug', 1),
('FEATURE', 'Feature Request', 1),
('SUPPORT', 'Support', 1),
('OTHER', 'Other', 1);
GO

-- Ticket Priorities
INSERT INTO mt_ticket_priority (code, name, sort_order)
VALUES
('LOW', 'Low', 1),
('MEDIUM', 'Medium', 2),
('HIGH', 'High', 3),
('CRITICAL', 'Critical', 4);
GO

-- Ticket Statuses
INSERT INTO mt_ticket_status (code, name, sort_order)
VALUES
('OPEN', 'Open', 1),
('IN_PROGRESS', 'In Progress', 2),
('RESOLVED', 'Resolved', 3),
('CLOSED', 'Closed', 4);
GO

-- Role Permission
INSERT INTO mt_rolepermission (role_id, permission_id)
SELECT 1, id
FROM mt_permission;