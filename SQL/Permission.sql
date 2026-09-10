use TicketingSystemDB
GO

INSERT INTO mt_permission(description, display, seq, sub_seq) VALUES
('ticket-index', 'View Ticket', 1, 1),
('ticket-details', 'Ticket Details', 1, 2),
('ticket-create', 'Create Ticket', 1, 3),
('ticket-update', 'Update Ticket', 1, 4),
('ticket-delete', 'Delete Ticket', 1, 5),
('ticket-page', 'Page Ticket', 1, 6),

('user-page', 'Page User', 2, 1),
('user-index', 'View User', 2, 2),
('user-details', 'User Details', 2, 3),
('user-create', 'User Create', 2, 4),
('user-update', 'User Update', 2, 3),
('user-toggleactive', 'User ToogleActive', 2, 4);