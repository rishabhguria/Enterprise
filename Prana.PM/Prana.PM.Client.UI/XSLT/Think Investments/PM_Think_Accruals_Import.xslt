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

  <xsl:template name="tempMonthName">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month = 'Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month = 'May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Jul'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$Month = 'Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL63"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'BNPH'"/>
            </xsl:variable>


			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="normalize-space(COL4)"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>

			  <Symbol>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					   <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>


			  <xsl:variable name="PB_FUND_NAME" select="''"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:value-of select ="'038QAG8G0 - BNPB'"/>
            </AccountName>

           
            <Date>
              <xsl:value-of select="''"/>
            </Date>

            <CurrencyName>
              <xsl:value-of select="'USD'"/>
            </CurrencyName>



            <xsl:variable name="varCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL63"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name = "Dramount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "Cramount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			  <xsl:variable name="DR_ACRONYM_NAME">
				  <xsl:choose>
					  <xsl:when test="$varCash &gt; 0">
						  <xsl:value-of select="'SwapInterestReceivable'"/>
					  </xsl:when>

					  <xsl:when test="$varCash &lt; 0">
						  <xsl:value-of select="'SwapInterestExpense'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="CR_ACRONYM_NAME">
				  <xsl:choose>
					  <xsl:when test="$varCash &gt; 0">
						  <xsl:value-of select="'SwapInterestIncome'"/>
					  </xsl:when>
					  <xsl:when test="$varCash &lt; 0">
						  <xsl:value-of select="'SwapInterestPayable'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <JournalEntries>
              <xsl:choose>
                <xsl:when test="number($Dramount)">
                  <xsl:value-of select="concat($DR_ACRONYM_NAME,':', $Dramount , '|',$CR_ACRONYM_NAME, ':' , $Cramount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            
            <Description>
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="'Swap Interest Receivable'"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="'Swap Interest Payable'"/>
				</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
             
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>