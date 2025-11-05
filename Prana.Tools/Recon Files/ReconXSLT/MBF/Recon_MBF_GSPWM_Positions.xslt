<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			
			<xsl:variable name = "varDate">
				<xsl:value-of select="PositionMaster[substring-before(COL1,'of')='Positions as ']/COL1"/>
			</xsl:variable>			
			<xsl:for-each select="Comparision">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL2"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='US Equity' or $varInstrumentType ='Non-US Equity'">

					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSPWM']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
						</xsl:choose>
						<xsl:variable name ="varQty" >
							<xsl:value-of select="translate(COL5,',','')"/>
						</xsl:variable>


						<xsl:choose>
							<xsl:when test ="$varQty &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="$varQty*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="$varQty &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="$varQty"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>											

						<AvgPX>
							<xsl:value-of select="translate(COL6,',','')"/>
						</AvgPX>

						<!-- Symbol Section-->

						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<CompanyName>
							<xsl:value-of select="COL3"/>
						</CompanyName>

						<xsl:choose>							
							<xsl:when test="starts-with(COL4,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL4)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL4,2,($varLength - 3)),' ',substring(COL4,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL4"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<!--<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
