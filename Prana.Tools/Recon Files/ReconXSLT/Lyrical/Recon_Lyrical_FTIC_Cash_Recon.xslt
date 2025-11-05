<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Currency">
		<xsl:param name="varCurrency"/>
		<xsl:choose>
			<xsl:when test="$varCurrency='U.S. Dollar'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="varMonth">
		<xsl:param name="MonthName"/>
		<xsl:choose>
			<xsl:when test="$MonthName='June'">
				<xsl:value-of select="6"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">
		<DocumentElement>

			<xsl:variable name="varFund" select="(//Comparision[COL1][1]/COL1[child::node()[1]])"/>

			<xsl:variable name="varDate" select="(//Comparision[COL1][4]/COL1[child::node()[1]])"/>

			<xsl:variable name="SymbolName" select="normalize-space(substring-after(//Comparision[contains(COL1,'Base Currency')]/COL1, ':'))"/>
			
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//Comparision">

				<xsl:variable name="varCashValueLocal" select="number(translate(translate(COL6,'$',''),',',''))"/>
				
				<xsl:if test ="$varCashValueLocal and normalize-space(COL1)='Cash' ">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'FTIC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="$varFund"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="LocalCurrency">
							<xsl:call-template name="Currency">
								<xsl:with-param name="varCurrency" select="$SymbolName"/>
							</xsl:call-template>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select="$LocalCurrency"/>
						</Symbol>


						<EndingQuantity>
							<xsl:choose>
								<xsl:when  test="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingQuantity>

						<xsl:variable name="MonthNo">
							<xsl:call-template name="varMonth">
								<xsl:with-param name="MonthName" select="substring-before($varDate,' ')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="vardateCode" select="substring-before(substring-after($varDate,' '),',')"/>

						<xsl:variable name="varYearCode" select="number(substring-after(substring-after($varDate,','),''))"/>

						<TradeDate>
							<xsl:value-of select="concat($MonthNo,'/',$vardateCode,'/',$varYearCode)"/>
						</TradeDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
