--BACKUP DATABASE SoftUni
--TO DISK = 'C:\Users\Tolev\Documents\SQL Server Management Studio\BackupDatabases\softuni-backup.bak'

--DROP DATABASE SoftUni

RESTORE DATABASE SoftUni 
FROM DISK = 'C:\Users\Tolev\Documents\SQL Server Management Studio\BackupDatabases\softuni-backup.bak'