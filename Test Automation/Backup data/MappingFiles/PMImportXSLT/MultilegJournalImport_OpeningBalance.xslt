<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test="not(contains(COL6, 'CR'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MapelRock'"/>
						</xsl:variable>

						<xsl:variable name = "PB_DR_ACRONYM_NAME">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

            <xsl:variable name="varPRANA_DR_ACRONYM_NAME">
              <xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@AccountName=$PB_DR_ACRONYM_NAME]/@Acronym"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_DR_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test ="$varPRANA_DR_ACRONYM_NAME=''">
                  <xsl:if test ="$PB_DR_ACRONYM_NAME = 'Collateral interest income [Income]'">
                    <xsl:value-of select="'Collateral interest income'"/>
                  </xsl:if>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varPRANA_DR_ACRONYM_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
                     
						<xsl:variable name="PB_FUND_NAME" select="COL2"/>
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

						<xsl:variable name="Date" select="COL1"/>
						<Date>
							<xsl:value-of select="$Date"/>
						</Date>
				
						<CurrencyName>
							<xsl:value-of select="COL3"/>
						</CurrencyName>

						<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>
						
						<xsl:variable name="varDrCash">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCrCash">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name = "Dramount" >
							<xsl:choose>
								<xsl:when test="$varDrCash &gt; 0">
									<xsl:value-of select="$varDrCash"/>
								</xsl:when>
								<xsl:when test="$varDrCash &lt; 0">
									<xsl:value-of select="$varDrCash*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "Cramount" >
							<xsl:choose>
								<xsl:when test="$varCrCash &gt; 0">
									<xsl:value-of select="$varCrCash"/>
								</xsl:when>
								<xsl:when test="$varCrCash &lt; 0">
									<xsl:value-of select="$varCrCash*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<Cash-SubAccount>
							<xsl:choose>
								<xsl:when test="$PRANA_DR_ACRONYM_NAME!=''">
									<xsl:value-of select="$PRANA_DR_ACRONYM_NAME"/>
								</xsl:when>
							</xsl:choose>
						</Cash-SubAccount>
						
						<AccountSide>
							<xsl:value-of select="COL9"/>
						</AccountSide>
						
						<DR>
							<xsl:value-of select="$Dramount"/>
						</DR>
						
						<CR>
							<xsl:value-of select="$Cramount"/>
						</CR>
												
						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>
						
						<FXRate>
							<xsl:choose>
								<xsl:when test="$varFXRate &gt; 0">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>
						
						<FXConversionMethodOperator>
							<xsl:value-of select="COL11"/>
						</FXConversionMethodOperator>
						
						<xsl:variable name="varEntryID">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>
						
						<EntryID>
							<xsl:value-of select="$varEntryID"/>
						</EntryID>
						
						<xsl:variable name="Description" >
							<xsl:choose>
								<xsl:when test="COL10='*'">
									<xsl:value-of select="'No Description'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL7"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
					
						<Description>							
						<xsl:value-of select="$Description"/>													
						</Description>		
						
						<IsNonTradingTransaction>
							<xsl:value-of select="0"/>
						</IsNonTradingTransaction>
						
						<Symbol>
							<xsl:value-of select="COL12"/>
						</Symbol>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>