<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name = "varDate">
				<xsl:value-of select="PositionMaster[substring-before(COL1,'of')='Positions as ']/COL1"/>
			</xsl:variable>

			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL2"/>
				</xsl:variable>

				<xsl:if test ="$varInstrumentType ='Cash'">
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
						

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="COL4"/>
						</LocalCurrency>

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<xsl:variable name ="varCashValueLocal" >
							<xsl:value-of select="translate(COL5,',','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="boolean(number($varCashValueLocal))">
								<CashValueLocal>
									<xsl:value-of select="$varCashValueLocal"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="substring-after($varDate,'of')"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
