#Usage:
#powershell.exe -executionpolicy remotesigned -file ChangeProductVersion.ps1 2.3.0.0

if($args[0])
{
	$xml=New-Object XML;
	$xml.Load($PSScriptRoot + "\product.wxs");

	$xml.Wix.Product.Version = $args[0];

	$xml.Save($PSScriptRoot + "\product.wxs");
}