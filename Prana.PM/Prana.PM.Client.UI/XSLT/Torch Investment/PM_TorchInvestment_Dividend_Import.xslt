<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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

			
				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL33"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="varISIN" select="COL24"/>
            
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varISIN !='*' or $varISIN !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            <ISIN>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varISIN!='*' or $varISIN!=''">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>

						<xsl:variable name="varEXDate">
              <xsl:value-of select="substring(COL29,7,2)"/>
						</xsl:variable>

						<xsl:variable name="varEXMonth">
              <xsl:value-of select="substring(COL29,5,2)"/>
						</xsl:variable>

						<xsl:variable name="varEXYear">
              <xsl:value-of select="substring(COL29,1,4)"/>
						</xsl:variable>

						<xsl:variable name="varEXFULLDATE">
							<xsl:value-of select="concat($varEXMonth,'/',$varEXDate,'/',$varEXYear)"/>
						</xsl:variable>

						<ExDate>
							<xsl:value-of select="$varEXFULLDATE" />
						</ExDate>


            <xsl:variable name="varYYYY">
              <xsl:value-of select="substring(COL29,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varMM">
              <xsl:value-of select="substring(COL29,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varDD">
              <xsl:value-of select="substring(COL29,7,2)"/>
            </xsl:variable>

						<xsl:variable name="varPAYFULLDATE">
							<xsl:value-of select="concat($varMM,'/',$varDD,'/',$varYYYY)"/>
						</xsl:variable>

						<PayoutDate>
							<xsl:value-of select="$varPAYFULLDATE" />
						</PayoutDate>
						

						<Amount>
							<xsl:choose>
								<xsl:when test="$varAmount &gt; 0 ">
									<xsl:value-of select="$varAmount" />
								</xsl:when>
								<xsl:when test="$varAmount &lt; 0 ">
									<xsl:value-of select="$varAmount * (-1)" />
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
						</Amount>

						<xsl:variable name="varActivityType">
							<xsl:choose>
								<xsl:when test="$varAmount &lt; 0 ">
									<xsl:value-of select="'DividendExpense'"/>
								</xsl:when>
								<xsl:when test="$varAmount &gt; 0 ">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<ActivityType>
							<xsl:value-of select="$varActivityType"/>
						</ActivityType>

            <Description>
              <xsl:choose>
                <xsl:when test="$varAmount &gt; 0">
                  <xsl:value-of select="'Dividend Received'" />
                </xsl:when>
                <xsl:when test="$varAmount &lt; 0">
                  <xsl:value-of select="'Dividend Charged'" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </Description>
						<FXRate>
							<xsl:choose>
								<xsl:when test="contains(COL21,'USD') ">
									<xsl:value-of select="'1'"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

         
            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="COL21"/>
            </xsl:variable>            
            <CurrencyName>
             <xsl:value-of select="$varCurrencyName"/>        
            </CurrencyName>

            <PBSymbol>
              <xsl:value-of select="COL12"/>
            </PBSymbol>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>