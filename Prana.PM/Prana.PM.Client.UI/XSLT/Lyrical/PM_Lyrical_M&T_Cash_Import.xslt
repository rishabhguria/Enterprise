<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>
			<xsl:variable name ="PB_NAME">
				<xsl:value-of select ="'M and T'"/>
			</xsl:variable>


			<xsl:for-each select ="//PositionMaster">
				
				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:value-of select="normalize-space(substring(COL1,2,11))"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
				
				<xsl:if test ="$PRANA_FUND_NAME!='' and normalize-space(substring(COL1,221,11))='WGOXX' or (normalize-space(substring(COL1,221,11))='TOIXX' and normalize-space(substring(COL1,2,11))='3117816-000')">

					<PositionMaster>						

						<AccountName>

							<xsl:choose>

								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose >

						</AccountName>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="substring(COL1,791,3)"/>
						</LocalCurrency>


						<xsl:variable name="varCashValueLocal">
							<xsl:choose>
								<xsl:when test="contains(normalize-space(substring(COL1,2,11)),'31048172')">
									<xsl:value-of select="normalize-space(substring(COL1,89,9)) + normalize-space(substring(COL1,105,7)) + normalize-space(substring(COL1,127,16))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(substring(COL1,127,16))"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="normalize-space(substring(COL1,89,9)) + normalize-space(substring(COL1,105,7)) + normalize-space(substring(COL1,127,16))"/>-->
						</xsl:variable>

						<CashValueLocal>
							<xsl:choose>

								<xsl:when test ="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</CashValueLocal>

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<Date>
							<xsl:value-of select ="concat(substring(COL1,663,2),'/',substring(COL1,665,2),'/',substring(COL1,659,4))"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
