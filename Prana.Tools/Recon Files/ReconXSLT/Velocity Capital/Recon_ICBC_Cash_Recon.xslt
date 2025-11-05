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
			<xsl:for-each select ="//Comparision">
				
				 <xsl:variable name="Cashlocal">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="(-1 * COL9)"/>
          </xsl:call-template>
        </xsl:variable>

			<xsl:if test ="number($Cashlocal)">
			
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Velocity'"/>
						</xsl:variable>



						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="concat(COL2,COL3,COL4,COL5,COL6)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								
								<xsl:when test ="COL6='*'">
							<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
								</xsl:when>
								<xsl:when test ="COL6=' '">
							<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
								</xsl:when>
								<xsl:when test ="COL6=''">
							<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<LocalCurrency>
							<xsl:value-of select="COL8"/>
						</LocalCurrency>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<Symbol>
							<xsl:value-of select="COL8"/>
						</Symbol>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test ="number($Cashlocal)">
									<xsl:value-of select="$Cashlocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CashValueLocal>

						
					


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>