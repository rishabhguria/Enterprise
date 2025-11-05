<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>



	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				
				<xsl:variable name="varCashValueLocal">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL16"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test ="$varCashValueLocal and (contains(COL1,'Wolfpack Capital Partners') or contains(COL7,'Dreyfus Treasury Securities') or COL7='CASH')">

					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="Import"/>
						</xsl:variable>
						
						<xsl:variable name="PB_FUND_NAME" select="COL4"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=Import]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="COL13"/>
						</LocalCurrency>
						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when  test="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>
<xsl:variable name="inputDate" select="normalize-space(COL22)"/>

<xsl:variable name="Year" select="substring($inputDate, 1, 4)"/>
<xsl:variable name="Month" select="substring($inputDate, 5, 2)"/>
<xsl:variable name="Day" select="substring($inputDate, 7, 2)"/>

<xsl:variable name="FormattedDate">
  <xsl:value-of select="concat($Month, '/', $Day, '/', $Year)"/>
</xsl:variable>

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
						<!-- this an optional field, whenever currency value needs to adjust, 
						set its value yes, else no or if we do not require adjustment ,need not to include this tag in the XSLT file-->
						<!--<DataAdjustReq>
							<xsl:value-of select ="'yes'"/>
						</DataAdjustReq>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
