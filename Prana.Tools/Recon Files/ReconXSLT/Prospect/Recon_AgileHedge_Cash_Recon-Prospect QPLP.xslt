<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
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
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="CashLocal">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($CashLocal) and (normalize-space(COL7)='CANADIAN DOLLAR - GSEC' or normalize-space(COL7)='U.S. DOLLAR - GSCO')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Agile'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="'Prospect - QP LP - BTIG'"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
							<xsl:value-of select="substring-before(COL9,'_')"/>
						</Symbol>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($CashLocal)">
									<xsl:value-of select="$CashLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>

						<xsl:variable name="FXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="CashValueBase">
							<xsl:choose>
								<xsl:when test="number($FXRate)">
									<xsl:value-of select="$CashLocal * $FXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$CashLocal"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CashValueBase>
							<xsl:choose>
								<xsl:when test="number($CashValueBase)">
									<xsl:value-of select="$CashValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueBase>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>