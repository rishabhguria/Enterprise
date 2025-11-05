<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="varCurrency">
		<xsl:param name="Currency"/>
		<xsl:choose>
			<xsl:when test="$Currency='United States dollar'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:if test ="COL15='Cash and Short Term Investments' or COL15='Cash and Cash Equivalents'">

					<PositionMaster>

						<xsl:variable name ="PB_NAME">
							<xsl:value-of select ="'NorthernTrust'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
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
							</xsl:choose >

						</AccountName>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<xsl:variable name="varCurrency">
							<xsl:call-template name="varCurrency">
								<xsl:with-param name="Currency" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<LocalCurrency>
							<xsl:value-of select="$varCurrency"/>
						</LocalCurrency>


						<xsl:variable name="varCashValueLocal">
							<xsl:value-of select="number(COL5)"/>
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
							<xsl:value-of select ="COL3"/>
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