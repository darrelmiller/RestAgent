$package =  (gci packages\Tavis.RestAgent.*.nupkg | sort-object -descending | select-object -first 1)
nuget push $package b56046b6-f1de-49f0-887e-27236a807a33 
copy-item $package \\granite\tavisproducts\nuget