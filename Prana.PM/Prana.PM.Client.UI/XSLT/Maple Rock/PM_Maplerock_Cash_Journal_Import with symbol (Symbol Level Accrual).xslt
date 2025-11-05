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

	<xsl:template name="ScientificToNumber">
		<xsl:param name="ScientificN"/>
		<xsl:variable name="vExponent" select="substring-after($ScientificN,'E')"/>
		<xsl:variable name="vMantissa" select="substring-before($ScientificN,'E')"/>
		<xsl:variable name="vFactor" select="substring('100000000000000000000000000000000000000000000',1, substring($vExponent,2) + 1)"/>
    <xsl:choose>
      <xsl:when test="starts-with($vExponent,'-')">
        <xsl:value-of select="$vMantissa div $vFactor"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$vMantissa * $vFactor"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test="not(contains(COL7, 'CR'))">

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
            
						<xsl:variable name = "PB_CR_ACRONYM_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="varPRANA_CR_ACRONYM_NAME">
							<xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@AccountName=$PB_CR_ACRONYM_NAME]/@Acronym"/>
						</xsl:variable>
            <xsl:variable name="PRANA_CR_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test ="$varPRANA_CR_ACRONYM_NAME=''">
                    <xsl:if test ="$PB_CR_ACRONYM_NAME = 'Collateral interest income [Income]'">
                      <xsl:value-of select="'Collateral interest income'"/>
                    </xsl:if>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varPRANA_CR_ACRONYM_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
                     
						<xsl:variable name="PB_FUND_NAME" select="COL2"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
				<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL9"/>
						</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
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
							<xsl:choose>
							<xsl:when test="contains(COL6,'E')">
								<xsl:call-template name="ScientificToNumber">
									<xsl:with-param name="ScientificN" select="COL6"/>
								</xsl:call-template>
							</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="Translate">
										<xsl:with-param name="Number" select="COL6"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>

						<xsl:variable name="varCrCash">
							<xsl:choose>
								<xsl:when test="contains(COL7,'E')">
									<xsl:call-template name="ScientificToNumber">
										<xsl:with-param name="ScientificN" select="COL7"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="Translate">
										<xsl:with-param name="Number" select="COL7"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
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
						
						<JournalEntries>	
							<xsl:choose>
								<xsl:when test="number($Dramount) and number($Cramount) and $PRANA_DR_ACRONYM_NAME!='' and $PRANA_CR_ACRONYM_NAME!=''">
									<xsl:value-of select="concat($PRANA_DR_ACRONYM_NAME,':', $Dramount , '|',$PRANA_CR_ACRONYM_NAME, ':' , $Cramount)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="Description" >
							<xsl:choose>
								<xsl:when test="COL8='*'">
									<xsl:value-of select="'No Description'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL8"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
					
						<Description>							
						<xsl:value-of select="$Description"/>													
						</Description>						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>