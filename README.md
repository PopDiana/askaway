## Facebook authentication setup 

In **Startup.cs** add the corresponding AppId and AppSecret: 

```
.AddFacebook(options => {
              options.AppId = "*****";
              options.AppSecret = "*****";
}
```


## Database setup

In **appsettings.json** modify the connection string:

`"DefaultConnection": "Server=*****\\SQLEXPRESS;Database=askaway_Database;Trusted_Connection=True;MultipleActiveResultSets=true"`

Powershell (add a new migration):

```

$ add-migration migrationName
$ update-database

```

If necessary, remove the latest migration with:

`$ remove-migration`

Login credentials:

```

email: aa@aa.aa
password: P@$$w0rd

```

