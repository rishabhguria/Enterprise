<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>



			<xsl:for-each select="Comparision">
				<xsl:if test =" COL8 = 'Mutual Fund' and number(COL12)">
					<PositionMaster>

						<xsl:variable name ="PB_NAME">
							<xsl:value-of select ="'Fidelity'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="varBaseCurrency">
							<xsl:value-of select="'USD'"/>
						</xsl:variable>

						<xsl:variable name="varLocalCurrency">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name="varCashValueLocal">
							<xsl:value-of select="COL12"/>
						</xsl:variable>

						<xsl:variable name="varCashValueBase">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varDate">
							<xsl:value-of select="''"/>
						</xsl:variable>



						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>



						<!--<Currency>
							<xsl:value-of select="$varBaseCurrency"/>
						</Currency>-->

						<Symbol>
							<xsl:value-of select="$varLocalCurrency"/>
						</Symbol>

						<EndingQuantity>
							<xsl:choose>
								<xsl:when test ="boolean(number($varCashValueLocal))">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</EndingQuantity>

						<!--<BeginningQuantity>
							<xsl:choose>
								<xsl:when test ="boolean(number($varCashValueLocal))">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</BeginningQuantity>-->

						<!--<Date>
							<xsl:value-of select="''"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>-->


					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
