compact-gzip:
	tar --exclude='src/Setember/obj/*' --exclude='src/Setember/bin/*' --exclude='src/Setember.Decrypter/obj/*' --exclude='src/Setember.Decrypter/bin/*'  -czvf setember.tar.gz ../Setember 

compact-bz2:
	tar --exclude='src/Setember/obj/*' --exclude='src/Setember/bin/*' --exclude='src/Setember.Decrypter/obj/*' --exclude='src/Setember.Decrypter/bin/*'  -cjvf setember.tar.bz2 ../Setember 
