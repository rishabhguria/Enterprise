<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>



	<xsl:template match="/">
		<DocumentElement>

			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//Comparision">
				
				<xsl:variable name ="varSign">
					<xsl:value-of select="substring(COL1,17,1)"/>
				</xsl:variable>
				
				<xsl:variable name ="varCashValueLocal">
					<xsl:choose>
						<xsl:when test ="$varSign='-'">
							<xsl:value-of select ="(-1) * number(substring(COL1,18,14))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="number(substring(COL1,18,14))"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test =" substring(COL1,32,9) ='90265M102' or substring(COL1,32,9)='751990441' or (number($varCashValueLocal) and (substring(COL1,8,6)='919136' or substring(COL1,8,6)='919446' or substring(COL1,8,6)='919195'or substring(COL1,8,6)='919101'or substring(COL1,8,6)='D58571'or  substring(COL1,8,4)='CASH'))">

					<PositionMaster>


						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="substring(COL1,1,7)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>
					
						<Symbol>
							<xsl:value-of select="'USD'"/>
						</Symbol>


						<EndingQuantity>
							<xsl:choose>

								<xsl:when test ="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</EndingQuantity>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
