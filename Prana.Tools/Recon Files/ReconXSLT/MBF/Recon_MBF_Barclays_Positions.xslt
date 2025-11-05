<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL1"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='EQUITY'">

					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL28,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Barclays']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:choose>
							<xsl:when test ="COL11 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL11*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL11 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL11"/>
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
							<xsl:value-of select="COL13"/>
						</AvgPX>

						<!-- Symbol Section-->

						<PBSymbol>
							<xsl:value-of select="COL3"/>
						</PBSymbol>

						<CompanyName>
							<xsl:value-of select="COL4"/>
						</CompanyName>

						<xsl:variable name ="varRIC">
							<xsl:value-of select ="substring-after(COL3,'.')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varRIC != ''">								
								<Symbol>
									<xsl:value-of select="substring-before(COL3,'.')"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="starts-with(COL3,'Q') and COL2 = 'Options'">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL3)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL3,2,($varLength - 3)),' ',substring(COL3,($varLength - 1),$varLength))"/>
								</Symbol>								
							</xsl:when>
							<xsl:otherwise>								
								<Symbol>
									<xsl:value-of select="COL3"/>
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
