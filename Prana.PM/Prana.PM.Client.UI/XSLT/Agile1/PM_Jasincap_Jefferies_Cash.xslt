<?xml version="1.0" encoding="UTF-8"?>

<!--Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="contains(COL4,'Cash') and number(COL8)">
					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Agile']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>



						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="normalize-space(COL5)"/>
						</LocalCurrency>

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL8))">
								<CashValueLocal>
									<xsl:value-of select="COL8"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose>

						<Date>
							<xsl:value-of select="''"/>
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
