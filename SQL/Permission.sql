use TicketingSystemDB
GO

INSERT INTO mt_permission(description, display, seq, sub_seq) VALUES
('ticket-index',   'View Ticket',    1, 1),
('ticket-details', 'Ticket Details', 1, 2),
('ticket-create',  'Create Ticket',  1, 3),
('ticket-update',  'Update Ticket',  1, 4),
('ticket-delete',  'Delete Ticket',  1, 5),
('ticket-page',   'Page Ticket',    1, 6);