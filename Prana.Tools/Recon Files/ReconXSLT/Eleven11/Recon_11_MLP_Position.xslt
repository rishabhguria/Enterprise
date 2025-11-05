<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL27"/>
				</xsl:variable>
				<xsl:if test ="($varInstrumentType ='Stock' or $varInstrumentType ='Equity ADR' or $varInstrumentType ='Future' or $varInstrumentType ='Option' or $varInstrumentType ='Future Option') and COL8 !='Quantity' and COL8 != 0">
					<PositionMaster>
						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="translate(COL19,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MLP']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<!--  Symbol Region -->
						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL3='USD' and ($varInstrumentType ='Stock' or $varInstrumentType ='Equity ADR')">
										<xsl:variable name ="strBeforeRIC">
											<xsl:value-of select="substring-before(COL9,'.')"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test ="$strBeforeRIC=''">
												<Symbol>
													<xsl:value-of select="COL9"/>
												</Symbol>												
											</xsl:when>
											<xsl:otherwise>
												<Symbol>
													<xsl:value-of select="$strBeforeRIC"/>
												</Symbol>											
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test ="$varInstrumentType ='Future'">
										<xsl:variable name = "varLength" >
											<xsl:value-of select="string-length(COL9)"/>
										</xsl:variable>
										<xsl:variable name = "varAfter" >
											<xsl:value-of select="substring(COL9,($varLength)-1,2)"/>
										</xsl:variable>
										<xsl:variable name = "varBefore" >
											<xsl:value-of select="substring(COL9,1,($varLength)-2)"/>
										</xsl:variable>
										<Symbol>
											<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
										</Symbol>										
									</xsl:when >
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>										
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						
						<PBSymbol>
							<xsl:value-of select="COL9"/>
						</PBSymbol>


						<xsl:choose>
							<xsl:when test="COL8 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL8*(-1)"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL8"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL30))">
								<AvgPX>
									<xsl:value-of select="COL30"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<CompanyName>
							<xsl:value-of select="COL19"/>
						</CompanyName>

						<!--fundname section-->
						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SCMS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>-->

						<FundName>
							<xsl:value-of select="'OFFSHORE'"/>
						</FundName>

						<!--AssetName section-->
						<!--<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='SCUBS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>-->
						<PBAssetName>
							<xsl:value-of select='$varInstrumentType'/>
						</PBAssetName>

						<!--<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>-->
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->

</xsl:stylesheet>
