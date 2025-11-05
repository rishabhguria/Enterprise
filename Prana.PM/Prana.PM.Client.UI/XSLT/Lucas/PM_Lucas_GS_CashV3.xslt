<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name = "varDate">
				<xsl:value-of select="PositionMaster[COL1='Business Date:']/COL2"/>
			</xsl:variable>
			<xsl:variable name = "varPortfolioID">
				<xsl:value-of select="PositionMaster[COL1='Fund:']/COL2"/>
			</xsl:variable>
			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL1,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType != 'Cash' and  $varInstrumentType != 'Currency Hedge'">
					<PositionMaster>
						<!--   Fund -->
						<xsl:choose>
							<xsl:when test="$varPortfolioID='LUCAS TOP'">
								<FundName>
									<xsl:value-of select="'LP C/O'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						<!--<xsl:choose>
							<xsl:when test ="COL7 &lt; 0 or COL7 &gt; 0 or COL7 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL7"/>
								</CashValueLocal>
								<CashValueBase>
									<xsl:value-of select="COL7"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL7))">
								<CashValueLocal>
									<xsl:value-of select="COL7"/>
								</CashValueLocal>
								<CashValueBase>
									<xsl:value-of select="COL7"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >
						<Date>
							<xsl:value-of select="$varDate"/>
						</Date>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
						<!-- this an optional field, whenever currency value needs to adjust, 
						set its value yes, else no or we need not to include it in the XSLT file-->
						<DataAdjustReq>
							<xsl:value-of select ="'no'"/>
						</DataAdjustReq>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
