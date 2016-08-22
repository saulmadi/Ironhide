$json = (Get-Content "localconfig.json" -Raw) | ConvertFrom-Json

$jsos.psobject.properties.name

foreach ($propertyName in $json.psobject.properties.name) {
    $propertyValue = $json.$propertyName
    write  $propertyName   $propertyValue
    [Environment]::SetEnvironmentVariable($propertyName, $propertyValue, "User")
}

