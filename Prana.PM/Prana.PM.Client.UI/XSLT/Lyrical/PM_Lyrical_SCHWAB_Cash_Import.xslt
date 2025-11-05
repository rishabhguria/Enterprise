<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<!--   Fund -->
				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:value-of select="COL1"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='SCHWAB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:variable name="varQuantity">
					<xsl:choose>
						<xsl:when test="contains(COL1,'46370395')">
							<xsl:value-of select="COL7"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL7"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varCase">
					<xsl:choose>
						<xsl:when test="contains(COL1,'46370395')">
							<xsl:value-of select="COL4"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL4"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
				<xsl:if test ="number($varQuantity) and contains($varCase,'ca') and  $PRANA_FUND_NAME!=''">

					<PositionMaster>						
						
						

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>


						<xsl:variable name="CashValueLocal">
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL7"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($CashValueLocal)">
									<xsl:value-of select="number($CashValueLocal)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</CashValueLocal>

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<Date>
							<xsl:value-of select="COL10"/>
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
