<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL2"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='US Equity' or $varInstrumentType ='Non-US Equity' or $varInstrumentType ='Commodities' or $varInstrumentType = 'Global Equity'">

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
							<xsl:when test ="boolean(number($varQty))">
								<Quantity>
									<xsl:value-of select="$varQty"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL3 = 'Buy'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when >
							<xsl:when test ="COL3 = 'Short Sale'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when >
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose >

						<AvgPX>
							<xsl:value-of select="translate(COL8,',','')"/>
						</AvgPX>

						<!-- Symbol Section-->

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when test="starts-with(COL6,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL6)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL6,2,($varLength - 3)),' ',substring(COL6,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL6	"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<CompanyName>
							<xsl:value-of select="COL7"/>
						</CompanyName>

						<!--GROSS NOTIONAL-->
						<GrossNotionalValue>
							<xsl:value-of select="translate(COL12,',','')"/>
						</GrossNotionalValue>

						<!--COMMISSION-->
						<xsl:variable name ="varCommission">
							<xsl:value-of select="translate(COL12,',','') - translate(COL9,',','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varCommission &lt; 0">
								<Commission>
									<xsl:value-of select='format-number($varCommission*(-1), "###.0000")'/>
								</Commission>
							</xsl:when>
							<xsl:when test ="$varCommission &gt; 0">
								<Commission>
									<xsl:value-of select='format-number($varCommission, "###.0000")'/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<Fees>
							<xsl:value-of select="0"/>
						</Fees>

						<!--NET NOTIONAL-->
						<NetNotionalValue>
							<xsl:value-of select="translate(COL9,',','')"/>
						</NetNotionalValue>

						<!--<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
