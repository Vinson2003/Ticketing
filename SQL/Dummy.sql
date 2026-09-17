USE TicketingSystemDB;
GO

INSERT INTO mt_user
(username, password, name, email, role_id, is_active)
VALUES
('admin', 'dummy-password', 'System Administrator', 'admin@ticketing.local', 1, 1);
GO

INSERT INTO tr_ticket
(ticket_no, title, description, category_id, priority_id, status_id, created_by, assigned_to)
VALUES
('TCK-000001', 'Unable to login', 'User cannot login after changing password.', 1, 3, 1, 1, NULL);
GO